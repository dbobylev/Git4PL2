using Git4PL2.Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    interface IPluginSettingsStorage
    {
        IPluginParameter GetParam(ePluginParameterNames name);

        T GetParamValue<T>(ePluginParameterNames name);

        IEnumerable<PluginParameterGroup> GetGroups { get; }

        IEnumerable<IPluginParameter> ParametersByGroup(ePluginParameterGroupType GroupType);
    }
}
