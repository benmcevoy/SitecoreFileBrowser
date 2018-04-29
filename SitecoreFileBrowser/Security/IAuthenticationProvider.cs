using System.Web;

namespace SitecoreFileBrowser.Security
{
	public interface IAuthenticationProvider
	{
		string GetChallengeToken();
		SecurityState ValidateRequest(HttpRequestBase request);
	    SuperWebClient CreateAuthenticatedWebClient(string url);
	}
}