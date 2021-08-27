using Git4PL2.Abstarct;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using Git4PL2.Plugin.Commands;
using Git4PL2.Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin
{
    class PluginCommands : IPluginCommands
    {
        private readonly IIDEProvider _IDEProvider;

        public PluginCommands(IIDEProvider IDEProvider)
        {
            _IDEProvider = IDEProvider;
        }

        private void RunCommand(PluginCommand command, object param = null)
        {
            Seri.SetModule(command.Name);
            Seri.Log.Here().Debug($"Run command {command.Name}");

            try
            {
                command.Execute(param);

            }
            catch (Exception ex)
            {
                Seri.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Seri.Log.Here().Debug($"End command {command.Name}");
            Seri.SetModule(string.Empty);
        }

        public void SaveTextToRepository(TextOperationsParametrs param = null)
        {
            RunCommand(NinjectCore.Get<CommandSaveTextToRepository>(), param);
        }

        public void LoadTextFromRepository(TextOperationsParametrs param = null)
        {
            RunCommand(NinjectCore.Get<CommandLoadTextFromRepository>(), param);
        }

        public void ShowGitDiff()
        {
            RunCommand(NinjectCore.Get<CommandShowGitDiff>());
        }

        public void ShowSettings()
        {
            RunCommand(NinjectCore.Get<CommandShowSettings>());
        }

        public void ShowGitBlame()
        {
            RunCommand(NinjectCore.Get<CommandShowGitBlame>());
        }

        public void ShowDicti()
        {
            RunCommand(NinjectCore.Get<CommandShowDicti>());
        }

        public void ShowTeamCoding()
        {
            RunCommand(NinjectCore.Get<CommandShowTeamCoding>());
        }

        public void CheckOut()
        {
            var dbObject = _IDEProvider.GetDbObject<IDbObject>(true);
            if (dbObject != null)
                RunCommand(NinjectCore.Get<CommandCheckOut>(), dbObject);
        }

        public void CheckIn()
        {
            var dbObject = _IDEProvider.GetDbObject<IDbObject>(true);
            if (dbObject != null)
                RunCommand(NinjectCore.Get<CommandCheckIn>(), dbObject);
        }

        public void ShowFtoggle()
        {
            RunCommand(NinjectCore.Get<CommandShowFtoggle>());
        }

        public void Help()
        {
            RunCommand(NinjectCore.Get<CommandOpenHelpPage>());
        }
    }
}
