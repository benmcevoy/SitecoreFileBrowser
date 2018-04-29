using System.Net;
using System.Web;

namespace SitecoreFileBrowser.Security
{
	public interface IAuthenticationProvider
	{
		string GetChallengeToken();
		SecurityState ValidateRequest(HttpRequestBase request);
		WebClient CreateAuthenticatedWebClient(string url);
	}
}