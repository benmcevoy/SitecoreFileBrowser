using SitecoreFileBrowser.Browse.Model;

namespace SitecoreFileBrowser.sitecore.admin.SitecoreFileBrowser
{
    public class CurrentMachine
    {
        public string Render(MachineInfo machine)
        {
            return $@"<h3 class='title'>{machine.Name}</h3><p class='subtitle'>{machine.Address}</p>";
        }
    }
}