using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    class CommandShowDicx: PluginCommand
    {
        private readonly IIDEProvider _IDEProvider;

        public CommandShowDicx(IIDEProvider IDEProvider) : base("PluginCommandShowDicx")
        {
            _IDEProvider = IDEProvider;
        }

        public override void Execute(object parameter)
        {
            var SelectedText = _IDEProvider.GetSelectedText().Trim();

            if (!Regex.IsMatch(SelectedText, @"^[a-z0-9_]+$", RegexOptions.IgnoreCase))
                throw new Exception($"Ошибочный текст для поиска в Dicx:\r\n{SelectedText}");

            WindowDicx wd = new WindowDicx(SelectedText);
            wd.Show();
        }

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
