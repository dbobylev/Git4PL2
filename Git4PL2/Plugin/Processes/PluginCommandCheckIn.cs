using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.TeamCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.Processes
{
    class PluginCommandCheckIn : PluginCommand
    {
        private ITeamCodingProvider _TeamCodingProvider;
        private ISettings _Settings;

        public PluginCommandCheckIn(ITeamCodingProvider TeamCodingProvider, ISettings Settings) : base("PluginCommandCheckOut")
        {
            _TeamCodingProvider = TeamCodingProvider;
            _Settings = Settings;
        }

        public override void Execute(object parameter)
        {
            if (parameter is IDbObject dbObject)
            {
                if (!_TeamCodingProvider.CheckIn(_Settings.TEAMCODING_LOGIN, dbObject, out string ErrorMsg))
                {
                    Seri.Log.Here().Error(ErrorMsg);
                    MessageBox.Show(ErrorMsg, "Team Coding Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show($"Сделан CheckIn для объекта {dbObject.ToString()}", "CheckIn", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public override bool CanExecute(object parameter)
        {
            return parameter != null && parameter is IDbObject;
        }
    }
}
