using Git4PL2.Abstarct;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.PLSqlDev
{
    class MenuItem : IMenuItem
    {
        public int Index { get; private set; }
        public string MenuName { get; private set; }
        public string MenuTip { get; private set; }
        public Action Click { get; private set; }
        public Bitmap Icon { get; private set; }

        public MenuItem(string menuName, string menuTip, Action click, Bitmap icon)
        {
            MenuName = menuName;
            Click = click;
            Icon = icon;
            MenuTip = menuTip;
        }
    }
}
