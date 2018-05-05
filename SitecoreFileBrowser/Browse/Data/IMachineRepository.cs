using System.Collections.Generic;
using SitecoreFileBrowser.Browse.Model;

namespace SitecoreFileBrowser.Browse.Data
{
    public interface IMachineRepository
    {
        IList<MachineInfo> Get();

        void Add(MachineInfo machine);

        void Delete(MachineInfo machine);
    }
}
