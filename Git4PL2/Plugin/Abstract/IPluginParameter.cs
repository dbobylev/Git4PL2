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

        ePluginParameterNames ParentParameter { get; }

        P GetValue<P>();
        void SetValue<P>(P value);
    }
}
