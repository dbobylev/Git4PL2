using Git4PL2.Abstarct;
using Git4PL2.Git.Abstract;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Processes
{
    class ShowGitDiff :PluginCommand
    {
        private IIDEProvider _IDEProvider;
        private IGitAPI _GitAPI;

        public ShowGitDiff(IIDEProvider IDEProvider, IGitAPI GitAPI) :base("ShowGitDiff")
        {
            _IDEProvider = IDEProvider;
            _GitAPI = GitAPI;
        }
        public override void PerformCommand()
        {
            IDbObjectText dbObj = _IDEProvider.GetDbObject<IDbObjectText>();
            List<string> rows = _GitAPI.GitDiff(dbObj);

            MyWindow w = new MyWindow(rows);
            w.Show();
        }
    }
}
