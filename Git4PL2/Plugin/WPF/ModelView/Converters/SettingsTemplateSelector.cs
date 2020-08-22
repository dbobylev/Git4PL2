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
        public DataTemplate SettingIntTemplate { get; set; }
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
                    case Settings.ePluginParameterUIType.Path:
                        return SettingsPathTemplate;
                    case Settings.ePluginParameterUIType.Text:
                        return SettingStringTemplate;
                    case Settings.ePluginParameterUIType.CheckBox:
                        return SettingsBoolTemplate;
                    case Settings.ePluginParameterUIType.List:
                        return SettingsListTemplate;
                    case Settings.ePluginParameterUIType.Number:
                        return SettingIntTemplate;
                    default:
                        break;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
