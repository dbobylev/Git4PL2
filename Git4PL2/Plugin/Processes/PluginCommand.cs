using Git4PL2.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Processes
{
    abstract class PluginCommand : IPluginCommand
    {
        public PluginCommand(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public virtual void PerformCommand()
        {
            throw new NotImplementedException("PerformCommand должен быть переопределен в дочернем классе");
        }
    }
}
