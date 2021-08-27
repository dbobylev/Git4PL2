using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    class CommandOpenHelpPage : PluginCommand
    {
        ISettings _Settings;

        public CommandOpenHelpPage(ISettings Settings) : base("help")
        {
            _Settings = Settings;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            System.Diagnostics.Process.Start(_Settings.HELPLINK);
        }
    }
}
