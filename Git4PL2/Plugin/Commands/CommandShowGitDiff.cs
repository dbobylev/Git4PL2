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
        private IIDEProvider _IDEProvider;

        public CommandShowGitDiff(IIDEProvider IDEProvider) :base("ShowGitDiff")
        {
            _IDEProvider = IDEProvider;
        }
        public override void Execute(object parameter)
        {
            IDbObjectText dbObj = _IDEProvider.GetDbObject<IDbObjectText>();

            WindowGitDiff WindowGitDiff = new WindowGitDiff(dbObj);
            WindowGitDiff.Show();
        }

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
