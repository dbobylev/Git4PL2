using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Abstarct
{
    interface IMenu
    {
        string CreateMenuItem(int index);

        void CreateToolButtons(int pluginID);

        void ClickOnMenu(int index);
    }
}
