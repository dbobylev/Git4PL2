using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Git4PL2.Plugin.WPF.ViewModel.Converters
{
     public class IsEnableSettingsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[1] == DependencyProperty.UnsetValue)
                return true;

            ePluginParameterNames Parent = (ePluginParameterNames)values[1];

            if (Parent == ePluginParameterNames.NULL)
                return true;

            IEnumerable Group = values[0] as IEnumerable;

            bool LoopFlag = false;
            while (!LoopFlag)
            {
                LoopFlag = true;

                // Ищем параметр родителя
                foreach (ParameterViewModel item in Group)
                {
                    // Если нашли проверяем его
                    if (item.ID == Parent)
                    {
                        if (!item.ValueBool)
                            return false;
                        else if (item.ParentParameter == ePluginParameterNames.NULL)
                            return true;
                        else
                        {
                            // Родитель включен и сам имеет параметр родитя, поднимаемся выше
                            Parent = item.ParentParameter;
                            LoopFlag = false;
                            break;
                        }
                    }
                }
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
