using Git4PL2.Plugin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Settings
{
    class PluginParameterList :PluginParameter<int>
    {
        public IEnumerable<string> ListItems { get; set; }

        public PluginParameterList(ePluginParameterID parameterName, int defaultValue, Type ListType) :base(parameterName, defaultValue)
        {
            ListItems = Helper.GetTypeDescriptions(ListType);
            ParamterUIType = ePluginParameterUIType.List;
        }
    }
}
