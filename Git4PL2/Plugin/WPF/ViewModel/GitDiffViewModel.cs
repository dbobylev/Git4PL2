using Git4PL2.Git.Abstract;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using Git4PL2.Plugin.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    class GitDiffViewModel : PropertyChangedBase
    {
        private IDbObjectText _DbObjectText;
        public List<Run> ListRuns { get; private set; }

        #region StatusBar
        public string CurrentBranch { get; private set; }
        public string CurrentDataBase { get; private set; }
        public string ObjectDescrName { get; private set; }
        public string ObjectFullPath { get; private set; }
        public bool? UnexpectedBranch { get; private set; }
        public bool? UnexpectedServer { get; private set; }

        #endregion

        #region Buttons
        public ICommand SaveTextCommand { get; private set; }
        public ICommand LoadTextCommand { get; private set; }
        public TextOperationsParametrs SaveTextCommandParam
        {
            get
            {
                return new TextOperationsParametrs() { DbObjectText = _DbObjectText, StringParam = CurrentBranch };
            }
        }
        public TextOperationsParametrs LoadTextCommandParam
        {
            get
            {
                return new TextOperationsParametrs() { DbObjectText = _DbObjectText, StringParam = CurrentDataBase };
            }
        }
        #endregion

        public bool ButtonsClassicStyle { get; private set; }

        public GitDiffViewModel(IDbObjectText DbObjectText, IIDEProvider IDE, IGitAPI Git, IWarnings Warnings, ISettings Settings)
        {
            _DbObjectText = DbObjectText;
            IDiffText DiffText = Git.GitDiff(DbObjectText);
            FillDocument(DiffText);

            CurrentBranch = Git.GetCurrentBranch();
            CurrentDataBase = IDE.GetDatabaseConnection();
            ObjectDescrName = DbObjectText.DescriptionName;
            ObjectFullPath = DbObjectText.GetRawFilePath();

            if (Settings.UnexpectedBranch)
                UnexpectedBranch = Warnings.IsBranchUnexsepted(CurrentBranch, true);
            if (Settings.UnexpectedServer)
                UnexpectedServer = Warnings.IsServerUnexsepted(CurrentDataBase, true);

            SaveTextCommand = NinjectCore.Get<CommandSaveTextToRepository>();
            LoadTextCommand = NinjectCore.Get<CommandLoadTextFromRepository>();

            ButtonsClassicStyle = Settings.ClassicButtonsPosition;
        }

        private void FillDocument(IDiffText DiffText)
        {
            var CurrentType = eDiffLineType.None;
            var sb = new StringBuilder();

            ListRuns = new List<Run>();
            while (DiffText.MoveNext())
            {
                if (DiffText.CurrentDiffLine.Type != CurrentType)
                {
                    if (CurrentType != eDiffLineType.None)
                    {
                        ListRuns.Add(new Run(sb.ToString())
                        {
                            Background = Helper.LineBackColor[CurrentType],
                            Foreground = Helper.LineTextColor[CurrentType]
                        });
                    }
                    CurrentType = DiffText.CurrentDiffLine.Type;
                    sb = new StringBuilder();
                }
                sb.Append(DiffText.CurrentLine);
            }

            if (!ListRuns.Any())
                ListRuns.Add(new Run("<Изменения отсутствуют>") { FontSize = 16 });

            OnPropertyChanged("ListRuns");
        }
    }
}
