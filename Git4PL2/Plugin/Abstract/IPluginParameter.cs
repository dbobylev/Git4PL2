using Git4PL2.Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    public interface IPluginParameter
    {
        ePluginParameterType ParamterType { get; }

        ePluginParameterNames Name { get; }

        string Description { get; }

        string DescriptionExt { get; }

        ePluginParameterGroupType Group { get; }

        int OrderPosition { get; }

        P GetValue<P>();

        void SetValue<P>(P value);

        string ValueString { get; set; }
        bool ValueBool { get; set; }
        int ValueInt { get; set; }
    }
}
