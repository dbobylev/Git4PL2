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
        private IIDEProvider _IDEProvider;

        public PluginCommandCheckIn(ITeamCodingProvider TeamCodingProvider, ISettings Settings, IIDEProvider IDEProvider) : base("PluginCommandCheckOut")
        {
            _TeamCodingProvider = TeamCodingProvider;
            _Settings = Settings;
            _IDEProvider = IDEProvider;
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
                    var OutputString = $"Сделан CheckIn для объекта {dbObject.ToString()}";
                    MessageBox.Show(OutputString, "CheckIn", MessageBoxButton.OK, MessageBoxImage.Information);
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
