using Git4PL2.Abstarct;
using Git4PL2.PLSqlDev.IDECallBacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.PLSqlDev
{
    class Menu : IMenu
    {
        private IIDECallbacks _IDECallbackl;

        private List<IMenuItem> MenuItems = new List<IMenuItem>();

        private string TabName { get; set; }
        private string GroupName { get; set; }
        private int? GroupIndex { get; set; }
        private bool? IsRibbonMenu { get; set; }
        private string GroupIndexStr
        {
            get
            {
                if (GroupIndex != null) return $" [groupindex={GroupIndex}]";
                else return "";
            }
        }

        public Menu(IIDECallbacks IDECallbacks)
        {
            _IDECallbackl = IDECallbacks;
            
            TabName = "Tools";
            GroupName = "Git4PL";
            GroupIndex = 2;

            //MenuItems.Add(new MenuItem("Тест", "Тест tip", () => { return; }, Properties.Resources.diskette));
        }

        public string CreateMenuItem(int index)
        {
            if (IsRibbonMenu == null)
                IsRibbonMenu = _IDECallbackl.GetDelegate<SYS_Version>().Invoke() >= 1200;

            if (IsRibbonMenu.Value && index == 1) return $"TAB={TabName}";
            if (IsRibbonMenu.Value && index == 2) return $"GROUP={GroupName}{GroupIndexStr}";

            var item = MenuItems.FirstOrDefault(x => x.Index == index);
            if (item != null)
            {
                if (IsRibbonMenu.Value)
                    return $"ITEM={item.MenuName}";
                else
                    return $"{TabName} / {item.MenuName}";
            }
            return null;
        }

        public void CreateToolButtons(int pluginID)
        {
            foreach (var item in MenuItems)
            {
                _IDECallbackl.GetDelegate<IDE_CreateToolButton>()?.Invoke(pluginID, item.Index, item.MenuName, null, item.Icon.GetHbitmap());
            }
        }

        public void ClickOnMenu(int index)
        {
            var item = MenuItems.FirstOrDefault(x => x.Index == index);
            if (item != null)
            {
                item.Click();
            }
        }
    }
}
