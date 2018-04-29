using System.IO;
using System.Security;
using Sitecore.Diagnostics;
using SitecoreFileBrowser.Browse;
using FileInfo = SitecoreFileBrowser.Browse.Model.FileInfo;

namespace SitecoreFileBrowser.Commands
{
    public class Download : Command
    {
        public Download() : base("Download"){ }

        public override CommandArguments Execute(CommandArguments args)
        {
            var path = args["path"].FromBase64();

            Log.Info($"SitecoreFileBrowser: Begin executing download '{path}'", this);
            
            var securityState =
                Configuration.AuthenticationProvider.ValidateRequest(args.HttpContext.Request);

            if (!securityState.IsAllowed) throw new SecurityException();

            var result = Configuration.FileBrowser.Download(new FileInfo { Path = args["path"] });

            args.HttpContext.Response.Clear();
            args.HttpContext.Response.ContentType = "application/octet-stream";
            args.HttpContext.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + Path.GetFileName(path) + "\"");
            args.HttpContext.Response.AddHeader("Content-Length", result.Length.ToString());

            result.CopyTo(args.HttpContext.Response.OutputStream);

            return args;
        }
    }
}