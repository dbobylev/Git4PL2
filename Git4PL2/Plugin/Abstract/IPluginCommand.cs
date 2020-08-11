using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Abstarct
{
    interface IPluginCommand
    {
        string Name { get; }
        void PerformCommand();
    }
}
