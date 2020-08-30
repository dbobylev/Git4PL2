using Git4PL2.Plugin.Abstract;
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
        private readonly IIDEProvider _IDEProvider;

        public CommandShowFtoggle(IIDEProvider IDEProvider):base("PluginCommandShowFtoggle")
        {
            _IDEProvider = IDEProvider;
        }

        public override void Execute(object parameter)
        {
            var SelectedText = _IDEProvider.GetSelectedText();

            if (!string.IsNullOrEmpty(SelectedText))
            {
                WindowFtoggle wf = new WindowFtoggle(SelectedText);
                wf.Show();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
