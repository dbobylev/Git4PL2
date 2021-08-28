using Git4PL2.Abstarct;
using Git4PL2.Git.Abstract;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.WPF;
using Git4PL2.Plugin.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    class CommandShowGitDiff :PluginCommand
    {
        public CommandShowGitDiff() :base("ShowGitDiff")
        {
        }
        public override void Execute(object parameter)
        {
            WindowGitDiff WindowGitDiff = NinjectCore.Get<WindowGitDiff>();
            WindowGitDiff.Show();
        }

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
