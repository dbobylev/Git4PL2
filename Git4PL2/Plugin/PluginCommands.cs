using Git4PL2.Abstarct;
using Git4PL2.Plugin.Model;
using Git4PL2.Plugin.Processes;
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
            RunCommand(NinjectCore.Get<PluginCommandSaveTextToRepository>(), param);
        }

        public void LoadTextFromRepository(TextOperationsParametrs param = null)
        {
            RunCommand(NinjectCore.Get<PluginCommandLoadTextFromRepository>(), param);
        }

        public void ShowGitDiff()
        {
            RunCommand(NinjectCore.Get<PluginCommandShowGitDiff>());
        }

        public void ShowSettings()
        {
            RunCommand(NinjectCore.Get<PluginCommandShowSettings>());
        }

        public void ShowGitBlame()
        {
            RunCommand(NinjectCore.Get<PluginCommandShowGitBlame>());
        }

        public void ShowDicti()
        {
            RunCommand(NinjectCore.Get<PluginCommandShowDicti>());
        }
    }
}
