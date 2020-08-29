using Git4PL2.Plugin.Diff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    static class Helper
    {
        public static readonly Dictionary<eDiffLineType, SolidColorBrush> LineTextColor = new Dictionary<eDiffLineType, SolidColorBrush>()
        {
            { eDiffLineType.Default, new SolidColorBrush(Color.FromRgb(0, 0, 0)) },
            { eDiffLineType.Plus, new SolidColorBrush(Color.FromRgb(0, 100, 0)) },
            { eDiffLineType.Minus, new SolidColorBrush(Color.FromRgb(139, 0, 0)) },
            { eDiffLineType.Mail, new SolidColorBrush(Color.FromRgb(128, 128, 128)) }
        };

        public static readonly Dictionary<eDiffLineType, SolidColorBrush> LineBackColor = new Dictionary<eDiffLineType, SolidColorBrush>()
        {
            { eDiffLineType.Default, new SolidColorBrush(Color.FromArgb(255,255,255,255)) },
            { eDiffLineType.Plus, new SolidColorBrush(Color.FromRgb(144, 238, 144)) },
            { eDiffLineType.Minus, new SolidColorBrush(Color.FromRgb(255, 182, 193)) },
            { eDiffLineType.Mail, new SolidColorBrush(Color.FromRgb(211, 211, 211)) }
        };
    }
}
