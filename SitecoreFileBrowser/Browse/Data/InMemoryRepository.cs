using System.Collections.Generic;
using System.Linq;
using SitecoreFileBrowser.Browse.Model;

namespace SitecoreFileBrowser.Browse.Data
{
    public class InMemoryMachineRepository : IMachineRepository
    {
        private static readonly Dictionary<string, MachineInfo> Machines = new Dictionary<string, MachineInfo>();

        public IList<MachineInfo> Get()
        {
            return Machines.Values.ToList();
        }

        public void Add(MachineInfo machine)
        {
            if (Machines.ContainsKey(machine.Address)) return;

            Machines.Add(machine.Address, machine);
        }

        public void Delete(MachineInfo machine)
        {
            if (!Machines.ContainsKey(machine.Address)) return;

            Machines.Remove(machine.Address);
        }
    }
}