using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.ModelView
{
    class SettingsViewModel : PropertyChangedBase
    {
        public IPluginSettingsStorage SettingStorages { get; private set; }

        public IEnumerable<PluginParameterGroup> Groups { get; private set; }

        public SettingsViewModel(IPluginSettingsStorage settingStorages)
        {
            SettingStorages = settingStorages;

            Groups = SettingStorages.GetGroups;
        }
    }
}
