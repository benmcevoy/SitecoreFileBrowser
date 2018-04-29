using System.IO;
using FileInfo = SitecoreFileBrowser.Browse.Model.FileInfo;

namespace SitecoreFileBrowser.Browse
{
    public interface IFileBrowser
    {
        Model.MachineInfo Browse();

        Stream Download(FileInfo file);
    }
}