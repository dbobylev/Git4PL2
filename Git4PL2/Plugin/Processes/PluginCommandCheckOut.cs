﻿using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.TeamCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.Processes
{
    class PluginCommandCheckOut : PluginCommand
    {
        private readonly ITeamCodingProvider _TeamCodingProvider;
        private readonly ISettings _Settings;
        private readonly IIDEProvider _IDEProvider;

        public PluginCommandCheckOut(ITeamCodingProvider TeamCodingProvider, ISettings Settings, IIDEProvider IDEProvider) : base("PluginCommandCheckOut")
        {
            _TeamCodingProvider = TeamCodingProvider;
            _Settings = Settings;
            _IDEProvider = IDEProvider;
        }

        public override void Execute(object parameter)
        {
            if (parameter is IDbObject dbObject)
            {
                if (!_TeamCodingProvider.CheckOut(_Settings.TEAMCODING_LOGIN, dbObject, out string ErrorMsg))
                {
                    Seri.Log.Here().Error(ErrorMsg);
                    MessageBox.Show(ErrorMsg, "Team Coding Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var OutputString = $"Сделан CheckOut для объекта {dbObject.ToString()}";
                    MessageBox.Show(OutputString, "CheckOut", MessageBoxButton.OK, MessageBoxImage.Information);
                    _IDEProvider.SetStatusMessage(OutputString);
                }
            }
        }

        public override bool CanExecute(object parameter)
        {
            return parameter != null && parameter is IDbObject;
        }
    }
}
