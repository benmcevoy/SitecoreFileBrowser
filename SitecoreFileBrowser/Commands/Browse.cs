using System.Security;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace SitecoreFileBrowser.Commands
{
    public class Browse : Command
    {
        public Browse() : base("Browse") { }

        public override CommandArguments Execute(CommandArguments args)
        {
            Log.Info("SitecoreFileBrowser: Begin executing browse", this);

            var securityState =
                Configuration.AuthenticationProvider.ValidateRequest(args.HttpContext.Request);

            if (!securityState.IsAllowed) throw new SecurityException();

            args.HttpContext.Response.Clear();
            args.HttpContext.Response.ContentType = "application.json";

            args.HttpContext.Response.Write(
                JsonConvert.SerializeObject(Configuration.FileBrowser.Browse(args["address"])));

            Log.Info("SitecoreFileBrowser: Finished executing browse", this);

            return args;
        }
    }
}