using Git4PL2.Abstarct;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.PLSqlDev
{
    class Menu : IEnumerable<IMenuItem>
    {
        private List<IMenuItem> MenuItems = new List<IMenuItem>();

        public Menu()
        {
            MenuItems.Add(new MenuItem("Тест", "Тест tip", () => { return; }, Properties.Resources.diskette));
        }

        public IEnumerator<IMenuItem> GetEnumerator()
        {
            foreach (IMenuItem item in MenuItems)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return MenuItems.GetEnumerator();
        }
    }
}
