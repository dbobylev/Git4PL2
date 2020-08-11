using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Model
{
    enum eSaveEncodingType
    {
        [Description("Сохранять в UTF-8")]
        UTF8 = 0,
        [Description("Сохранять в UTF-8 with BOM")]
        UTF8_BOM = 1,
        [Description("Не изменять существующий формат")]
        DontChange = 2
    }
}
