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
        ePluginParameterNames ID { get; }
        ePluginParameterGroupType Group { get; }
        ePluginParameterUIType ParamterUIType { get; }
        string Description { get; }
        string DescriptionExt { get; }
        int OrderPosition { get; }
        ePluginParameterNames ParentParameterID { get; }
        string ParentParameterStringValue { get; }
        string GetStringValue { get; }
        P GetValue<P>();
        void SetValue<P>(P value);
    }
}
