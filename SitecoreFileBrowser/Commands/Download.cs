using Sitecore.Diagnostics;
using SitecoreFileBrowser.Browse;
using SitecoreFileBrowser.Browse.Model;

namespace SitecoreFileBrowser.Commands
{
    public class Download : Command
    {
        public Download() : base("Download"){ }

        public override object Execute(CommandArguments args)
        {
            Log.Info($"SitecoreFileBrowser: Begin executing download '{args["path"].FromBase64()}", this);

            return Configuration.FileBrowser.Download(new FileInfo{ Path = args["path"]});
        }
    }
}