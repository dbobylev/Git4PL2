using Git4PL2.Git.Abstract;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using Git4PL2.Plugin.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.Commands
{
    class CommandShowGitBlame : PluginCommand
    {
        private readonly IGitAPI _GitAPI;
        private readonly IIDEProvider _IIDEProveider;
        private readonly ISettings _Settings;

        public CommandShowGitBlame(IIDEProvider IDEProvider, IGitAPI GitApi, ISettings Settings) : base("PluginCommandShowGitBlame")
        {
            _GitAPI = GitApi;
            _IIDEProveider = IDEProvider;
            _Settings = Settings;
        }

        public override void Execute(object parameter)
        {
            // Поулчаем объект БД открытый в PL/SQL Developer
            IDbObjectText DbObjectText = _IIDEProveider.GetDbObject<DbObjectTextRaw>();

            // Получаем номер строки, на которой установлен курсор
            int CurrentLineNumber = _IIDEProveider.GetCurrentLine();

            // Получаем номер соответствющей строки в репозитории
            int? RepositoryLineNumber = _GitAPI.GitDiffLineNumber(DbObjectText, CurrentLineNumber);

            if (RepositoryLineNumber != null)
            {
                int ValMinus = 5;
                int ValPlus = 5;

                if (_Settings.ShowGitBlameProperties)
                {
                    string[] TextLines = DbObjectText.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    WindowGitBlameProperties wgbp = new WindowGitBlameProperties(TextLines[(int)CurrentLineNumber - 1]);
                    bool? result = wgbp.ShowDialog();
                    if (result != null && (bool)result)
                    {
                        ValMinus = wgbp.OutputValMinus;
                        ValPlus = wgbp.OutputValPlus;
                    }
                    else
                    {
                        return;
                    }
                }

                var lines = _GitAPI.GitBlame<List<string>>(DbObjectText.RepName, Math.Max(1, (int)RepositoryLineNumber - ValMinus), (int)RepositoryLineNumber + ValPlus);
                WindowGitBlame wgb = new WindowGitBlame(lines);
                wgb.Show();
            }
            else
                MessageBox.Show("В репозитории не найдена данная строка (Если строка не является новой, попробуйте поменять ветку)", "Не найдена строка в репозитории", MessageBoxButton.OK, MessageBoxImage.Warning);

        }
        public override bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
