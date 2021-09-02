using Git4PL2.Abstarct;
using Git4PL2.PLSqlDev.IDECallBacks;
using Git4PL2.Plugin.Abstract;
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
        private readonly ICallbackManager _CallbackManager;
        private readonly IPluginCommands _PluginCommands;
        private readonly ISettings _Settings;
        private int _PluginId;

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

        public Menu(ICallbackManager IDECallbacks, IPluginCommands PluginCommands, ISettings Settings)
        {
            _CallbackManager = IDECallbacks;
            _PluginCommands = PluginCommands;
            _Settings = Settings;

            TabName = "Tools";
            GroupName = "Git4PL2";
            GroupIndex = 2;

            MenuItems.Add(new MenuItem("GitDiff", _PluginCommands.ShowGitDiff, Properties.Resources.diskette));
            MenuItems.Add(new MenuItem("Load", () => _PluginCommands.LoadTextFromRepository(), Properties.Resources.backred));
            MenuItems.Add(new MenuItem("Save", () => _PluginCommands.SaveTextToRepository(), Properties.Resources.forward));
            MenuItems.Add(new MenuItem("Blame", _PluginCommands.ShowGitBlame, Properties.Resources.trumpet));
            MenuItems.Add(new MenuItem("Dicti", _PluginCommands.ShowDicti, Properties.Resources.database_red_icon));
            MenuItems.Add(new MenuItem("Dicx", _PluginCommands.ShowDicx, Properties.Resources.database_red_icon));
            MenuItems.Add(new MenuItem("Ftoggle", _PluginCommands.ShowFtoggle, Properties.Resources.gear));
            MenuItems.Add(new MenuItem("TeamCoding", _PluginCommands.ShowTeamCoding, Properties.Resources.Categories_system_help_icon));
            MenuItems.Add(new MenuItem("CheckOut", _PluginCommands.CheckOut, Properties.Resources.Stock_Index_Down_icon));
            MenuItems.Add(new MenuItem("CheckIn", _PluginCommands.CheckIn, Properties.Resources.Stock_Index_Up_icon));
            MenuItems.Add(new MenuItem("Settings", _PluginCommands.ShowSettings, Properties.Resources.settings));
            MenuItems.Add(new MenuItem("Help", _PluginCommands.Help, Properties.Resources.Categories_system_help_icon));
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
                // Не отображаем меню TeamCoding если он отключен
                if (!_Settings.TEAMCODING_ENABLE 
                    && (  item.MenuName == "TeamCoding"
                       || item.MenuName == "CheckOut"
                       || item.MenuName == "CheckIn"))
                {
                    return null;
                }

                if (IsRibbonMenu.Value)
                    return $"ITEM={item.MenuName}";
                else
                    return $"{TabName} / {item.MenuName}";
            }
            return null;
        }

        public void CreateToolButtons(int pluginID)
        {
            _PluginId = pluginID;
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

        public void RefreshMenu()
        {
            _CallbackManager.GetDelegate<IDE_RefreshMenus>()?.Invoke(_PluginId);
            CreateToolButtons(_PluginId);
        }
    }
}
