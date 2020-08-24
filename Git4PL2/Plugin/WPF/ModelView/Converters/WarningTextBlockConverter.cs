using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Git4PL2.Plugin.WPF.ModelView.Converters
{
    class WarningTextBlockConverter :IValueConverter
    {
        public Style WarningAllowColorTextBlock { get; set; }
        public Style WarningErrorColorTextBlock { get; set; }
        public Style DefaultColorTextBlock { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DefaultColorTextBlock;

            if (value is bool boolValue)
                return boolValue ? WarningErrorColorTextBlock : WarningAllowColorTextBlock;

            return DefaultColorTextBlock;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
