using System;
using System.Web;
using Sitecore.Diagnostics;
using SitecoreFileBrowser.Browse.Model;

namespace SitecoreFileBrowser.Commands
{
    public class Browse : Command
    {
        public Browse() : base("Browse") { }

        public override object Execute(CommandArguments args)
        {
            Log.Info("SitecoreFileBrowser: Begin executing browse", this);

            var result = Cache(() => Configuration.FileBrowser.Browse());

            Log.Info("SitecoreFileBrowser: Finished executing browse", this);

            return result;
        }

        private static object Cache(Func<MachineInfo> f)
        {
            var cached = HttpContext.Current.Cache["__browse_ck"];

            if (cached != null) return cached;

            var result = f();

            if (result == null) return null;

            HttpRuntime.Cache.Insert("__browse_ck", result, null, 
                DateTime.Now.Add(TimeSpan.FromMinutes(Configuration.BrowseCacheInMinutes)), TimeSpan.Zero);

            return result;
        }
    }
}