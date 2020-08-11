
using Git4PL2.Abstarct;
using Git4PL2.PLSqlDev.IDECallBacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.IDE
{
    class Menu : IMenu
    {
        private ICallbackManager _CallbackManager;
        private IPluginCommands _PluginCommands;

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

        public Menu(ICallbackManager IDECallbacks, IPluginCommands PluginCommands)
        {
            _CallbackManager = IDECallbacks;
            _PluginCommands = PluginCommands;

            TabName = "Tools";
            GroupName = "Git4PL2";
            GroupIndex = 3;

            MenuItems.Add(new MenuItem("GitDiff", "Тест tip", _PluginCommands.ShowGitDiff, Properties.Resources.diskette));
            MenuItems.Add(new MenuItem("Load", "Тест tip", _PluginCommands.LoadTextFromRepository, Properties.Resources.Stock_Index_Down_icon));
            MenuItems.Add(new MenuItem("Save", "Тест tip", _PluginCommands.SaveTextToRepository, Properties.Resources.Stock_Index_Up_icon));
            
        }

        public string CreateMenuItem(int index)
        {
            if (IsRibbonMenu == null)
                IsRibbonMenu = _CallbackManager.GetDelegate<SYS_Version>().Invoke() >= 1200;

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
            Seri.Log.Here().Debug("CreateToolButtons  pluginID=" + pluginID);
            foreach (var item in MenuItems)
            {
                _CallbackManager.GetDelegate<IDE_CreateToolButton>()?.Invoke(pluginID, item.Index, item.MenuName, null, item.Icon.GetHbitmap());
            }
        }

        public void ClickOnMenu(int index)
        {
            var item = MenuItems.FirstOrDefault(x => x.Index == index);
            if (item != null)
            {
                Seri.Log.Here().Information("Click index " + index);
                item.Click();
            }
        }
    }
}
