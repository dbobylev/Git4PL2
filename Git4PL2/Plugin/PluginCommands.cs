using Git4PL2.Abstarct;
using Git4PL2.Plugin.Model;
using Git4PL2.Plugin.Processes;
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

        private void RunCommand(IPluginCommand command)
        {
            Seri.SetModule(command.Name);
            Seri.Log.Here().Debug($"Run command {command.Name}");

            try
            {
                command.PerformCommand();
            }
            catch (Exception ex)
            {
                Seri.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Seri.Log.Here().Debug($"End command {command.Name}");
            Seri.SetModule(string.Empty);
        }

        public void SaveTextToRepository()
        {
            RunCommand(NinjectCore.Get<SaveTextToRepository>());
        }

        public void LoadTextFromRepository()
        {
            RunCommand(NinjectCore.Get<LoadTextFromRepository>());
        }

        public void ShowGitDiff()
        {
            RunCommand(NinjectCore.Get<ShowGitDiff>());
        }
    }
}
