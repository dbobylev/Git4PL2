using Castle.DynamicProxy.Generators;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    class CommandShowTeamCoding :PluginCommand
    {
        public CommandShowTeamCoding(ISettings Settings) : base("PluginCommandShowTeamCoding")
        {
            if (string.IsNullOrEmpty(Settings.TEAMCODING_LOGIN))
            {
                throw new Exception("Не указан Логин для Team Coding");
            }
        }

        public override void Execute(object parameter)
        {
            WindowTeamCoding wtg = new WindowTeamCoding();
            wtg.Show();
        }

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
