using SitecoreFileBrowser.Browse.Model;

namespace SitecoreFileBrowser.sitecore.admin.SitecoreFileBrowser
{
    public class CurrentMachine
    {
        public string Render(MachineInfo machine)
        {
            return $@"<h3>{machine.Name} ({machine.Address})</h3>";
        }
    }
}