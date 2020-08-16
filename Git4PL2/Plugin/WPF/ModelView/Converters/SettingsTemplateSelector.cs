using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Git4PL2.Plugin.WPF.ModelView.Converters
{
    class SettingsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SettingStringTemplate { get; set; }
        public DataTemplate SettingsBoolTemplate { get; set; }
        public DataTemplate SettingsListTemplate { get; set; }
        public DataTemplate SettingsPathTemplate { get; set; }

        public SettingsTemplateSelector()
        {
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IPluginParameter Param)
            {
                switch (Param.ParamterType)
                {
                    case Settings.ePluginParameterType.Path:
                        return SettingsPathTemplate;
                    case Settings.ePluginParameterType.Text:
                        return SettingStringTemplate;
                    case Settings.ePluginParameterType.CheckBox:
                        return SettingsBoolTemplate;
                    case Settings.ePluginParameterType.List:
                        return SettingsListTemplate;
                    default:
                        break;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
