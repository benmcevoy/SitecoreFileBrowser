using Sitecore.Diagnostics;

namespace SitecoreFileBrowser.Commands
{
    public class Challenge :Command
    {
        public Challenge() : base("Challenge") { }

        public override CommandArguments Execute(CommandArguments args)
        {
            Log.Info($"SitecoreFileBrowser: Begin executing Challenge '{args}'", this);

            args.HttpContext.Response.Clear();
            args.HttpContext.Response.ContentType = "plain/text";
            args.HttpContext.Response.Write(Configuration.AuthenticationProvider.GetChallengeToken());

            Log.Info($"SitecoreFileBrowser: Finished executing Challenge '{args}'", this);

            return args;
        }
    }
}