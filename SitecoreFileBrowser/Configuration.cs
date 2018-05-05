using Sitecore.Configuration;
using SitecoreFileBrowser.Browse;
using SitecoreFileBrowser.Browse.Data;
using SitecoreFileBrowser.Security;

namespace SitecoreFileBrowser
{
    public static class Configuration
    {
        static Configuration()
        {
            Enabled = BoolOrDefault(Factory.GetString("/sitecore/sitecoreFileBrowser/enabled", true));
            AuthenticationProvider = (IAuthenticationProvider)Factory.CreateObject("/sitecore/sitecoreFileBrowser/authenticationProvider", true);
            Repository = (IMachineRepository)Factory.CreateObject("/sitecore/sitecoreFileBrowser/machineRepository", true);
            FileBrowser = (IFileBrowser)Factory.CreateObject("/sitecore/sitecoreFileBrowser/fileBrowser", true);
            Route = Factory.GetString("/sitecore/sitecoreFileBrowser/route", true);
        }

        private static bool BoolOrDefault(string value, bool defaultValue = false)
        {
            return bool.TryParse(value, out var result) ? result : defaultValue;
        }

        public static IMachineRepository Repository { get; set; }
        public static IAuthenticationProvider AuthenticationProvider { get; set; }
        public static IFileBrowser FileBrowser { get; set; }
        public static string Route { get; set; }
        public static bool Enabled { get; set; }
    } 
}