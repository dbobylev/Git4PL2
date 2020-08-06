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
        private static int _indexCounter = 10;

        public int Index { get; private set; }
        public string MenuName { get; private set; }
        public string MenuTip { get; private set; }
        private Action ActionClick { get; set; }
        public Bitmap Icon { get; private set; }

        public MenuItem(string menuName, string menuTip, Action click, Bitmap icon)
        {
            Index = _indexCounter++;
            MenuName = menuName;
            ActionClick = click;
            Icon = icon;
            MenuTip = menuTip;
        }

        public void Click()
        {
            ActionClick?.Invoke();
        }
    }
}
