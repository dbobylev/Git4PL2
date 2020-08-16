using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Settings
{
    public class PluginParameterGroup
    {
        public string Name { get; set; }

        public ePluginParameterGroupType GroupType { get; set; }

        public string Description { get; set; }
    }
}
