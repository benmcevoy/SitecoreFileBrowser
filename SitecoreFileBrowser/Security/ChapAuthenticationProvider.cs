using System;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using MicroCHAP;
using MicroCHAP.Server;

namespace SitecoreFileBrowser.Security
{
    public class ChapAuthenticationProvider : IAuthenticationProvider
    {
        private IChapServer _server;
        private ISignatureService _signatureService;
        private IChapServerLogger _chapServerLogger;

        public string SharedSecret { get; set; }
        public string ChallengeDatabase { get; set; } = "web";
        
        protected virtual IChapServer Server => _server ?? (_server = new ChapServer(SignatureService, ChallengeStore));

        protected virtual IChapServerLogger ServerLogger
        {
            get
            {
                if (_chapServerLogger == null)
                    return _chapServerLogger = new ChapServerSitecoreLogger("SitecoreFileBrowser-Auth");

                return _chapServerLogger;
            }
        }

        protected virtual ISignatureService SignatureService
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SharedSecret))
                    throw new SecurityException(
                        "The SitecoreFileBrowser shared secret is not set. Add a child <SharedSecret> element in the SitecoreFileBrowser <authenticationProvider> config (SitecoreFileBrowser.config) and set a secure shared secret, e.g. a 64-char random string.");

                return _signatureService ?? (_signatureService = new SignatureService(SharedSecret));
            }
        }

        protected virtual IChallengeStore ChallengeStore =>
            new SitecoreDatabaseChallengeStore(ChallengeDatabase, new ChallengeStoreSitecoreLogger("SitecoreFileBrowser-Auth"));
            

        public string GetChallengeToken()
        {
            ValidateSharedSecret();
            return Server.GetChallengeToken();
        }

        public SecurityState ValidateRequest(HttpRequestBase request)
        {
            var authToken = request.Headers["X-MC-MAC"];

            if (string.IsNullOrWhiteSpace(authToken)) return new SecurityState(false);

            ValidateSharedSecret();

            return Server.ValidateRequest(request, ServerLogger) 
                ? new SecurityState(true) 
                : new SecurityState(false);
        }

        public SuperWebClient CreateAuthenticatedWebClient(string url)
        {
            var remoteUri = new Uri(url);

            var client = new SuperWebClient(RequestTimeoutInMs);

            // we get a new challenge from the remote server, which is a unique known value to both parties
            var challenge = client.DownloadString(remoteUri.GetLeftPart(UriPartial.Path) + "?command=challenge");

            // then we sign the request using our shared secret combined with the challenge and the URL, providing a unique verifiable hash for the request
            client.Headers.Add("X-MC-MAC",
                SignatureService.CreateSignature(challenge, remoteUri.AbsoluteUri, Enumerable.Empty<SignatureFactor>())
                    .SignatureHash);

            // the server needs to know the challenge we are using. It makes sure that it issued the challenge before validating it.
            client.Headers.Add("X-MC-Nonce", challenge);

            return client;
        }

        protected virtual int RequestTimeoutInMs => 1000 * 60 * 120; // 2h in msec

        protected virtual void ValidateSharedSecret()
        {
            if (string.IsNullOrWhiteSpace(SharedSecret))
                throw new SecurityException(
                    "The SitecoreFileBrowser shared secret is not set. Add a child <SharedSecret> element in the SitecoreFileBrowser <authenticationProvider> config (SitecoreFileBrowser.config) and set a secure shared secret, e.g. a 64-char random string.");

            if (double.TryParse(SharedSecret, out _))
            {
                // if no shared secret is set we make it a random double, but we reject that once you actually try to authenticate with a tool
                throw new SecurityException(
                    "The SitecoreFileBrowser shared secret is not set, or was set to a numeric value. Add a child <SharedSecret> element in the SitecoreFileBrowser <authenticationProvider> config (SitecoreFileBrowser.config) and set a secure shared secret, e.g. a 64-char random string.");
            }
            
            if (SharedSecret.Length < 30)
                throw new SecurityException(
                    "Your SitecoreFileBrowser shared secret is not long enough. Please make it more than 30 characters for maximum security. You can set this in SitecoreFileBrowser.config on the <authenticationProvider>");
        }

        
    }

    public class SuperWebClient : WebClient
    {
        private readonly int _timeoutInMs;
        public readonly CookieContainer CookieContainer = new CookieContainer();

        public SuperWebClient(int timeoutInMs)
        {
            _timeoutInMs = timeoutInMs;
        }

        public void SetCookies(HttpCookieCollection httpCookies, string domain)
        {
            foreach (var k in httpCookies.AllKeys)
            {
                var originalCookie = HttpContext.Current.Request.Cookies[k];
                var cookie = new Cookie
                {
                    Name = originalCookie.Name,
                    Value = originalCookie.Value,
                    Domain = domain,
                    Expires = originalCookie.Expires
                };

                if (!string.IsNullOrWhiteSpace(originalCookie.Path)) cookie.Path = originalCookie.Path;

                CookieContainer.Add(cookie);
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);

            if (!(request is HttpWebRequest webRequest)) return request;

            webRequest.CookieContainer = CookieContainer;
            request.Timeout = _timeoutInMs;

            return request;
        }
    }
}
