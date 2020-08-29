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
    class CommandShowDicti :PluginCommand
    {
        private IIDEProvider _IDEProvider;

        public CommandShowDicti(IIDEProvider IDEProvider):base("PluginCommandShowDicti")
        {
            _IDEProvider = IDEProvider;
        }

        public override void Execute(object parameter)
        {
            var SelectedText = _IDEProvider.GetSelectedText().Trim();

            if (!Regex.IsMatch(SelectedText, @"^[a-z0-9_]+$", RegexOptions.IgnoreCase))
                throw new Exception($"Ошибочный текст для поиска в Dicti:\r\n{SelectedText}");

            WindowDicti wd = new WindowDicti(SelectedText);
            wd.Show();
        }

        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
