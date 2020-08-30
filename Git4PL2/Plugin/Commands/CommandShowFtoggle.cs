using Git4PL2.Plugin.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    class CommandShowFtoggle : PluginCommand
    {

        public CommandShowFtoggle():base("PluginCommandShowFtoggle")
        {

        }

        public override void Execute(object parameter)
        {
            WindowFtoggle wf = new WindowFtoggle();
            wf.Show();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
