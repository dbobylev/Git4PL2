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

        string GroupName { get; }

        P GetValue<P>();

        void SetValue<P>(P value);
    }
}
