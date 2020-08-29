﻿using Git4PL2.Abstarct;
using Git4PL2.Plugin.Abstract;
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
        private IIDEProvider _IDEProvider;

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

        public void ShowTeamCoding()
        {
            RunCommand(NinjectCore.Get<PluginCommandShowTeamCoding>());
        }

        public void CheckOut()
        {
            var dbObject = _IDEProvider.GetDbObject<IDbObject>(true);
            if (dbObject != null)
                RunCommand(NinjectCore.Get<PluginCommandCheckOut>(), dbObject);
        }

        public void CheckIn()
        {
            var dbObject = _IDEProvider.GetDbObject<IDbObject>(true);
            if (dbObject != null)
                RunCommand(NinjectCore.Get<PluginCommandCheckIn>(), dbObject);
        }
    }
}
