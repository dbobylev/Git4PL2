using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Git4PL2.Plugin.WPF.ViewModel.Converters
{
    class SettingsListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null)
                return null;

            if (values[0] is PluginParameterGroup Group && values[1] is IPluginSettingsStorage Storage)
            {
                return Storage.ParametersByGroup(Group.GroupType);
            }

            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
