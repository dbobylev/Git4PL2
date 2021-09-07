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
using System.Text.RegularExpressions;
using Git4PL2.Plugin.Settings;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    class GitDiffViewModel : PropertyChangedBase
    {
        private IDbObjectText _DbObjectText;
        private IIDEProvider _IDEProvider;
        private IPluginSettingsStorage _PluginSettingsStorage;
        public List<Run> ListRuns { get; private set; }

        #region StatusBar
        public string CurrentBranch { get; private set; }
        public string CurrentDataBase { get; private set; }
        public string ObjectDescrName { get; private set; }
        public string ObjectFullPath { get; private set; }
        public bool? UnexpectedBranch { get; private set; }
        public bool? UnexpectedServer { get; private set; }

        private bool _GoToLineChecked = true;
        public string GoToLineContent { get; private set; }
        public bool GoToLineChecked
        {
            get => _GoToLineChecked;
            set
            {
                _GoToLineChecked = value;
                OnPropertyChanged();
                if (_GoToLineChecked)
                    GoToLineContent = "Go to line: on";
                else
                    GoToLineContent = "Go to line: off";
                OnPropertyChanged("GoToLineContent");

                // Сохраняем значение параметра
                IPluginParameter GoToLineParam = _PluginSettingsStorage.GetParam(ePluginParameterID.GoToLine);
                GoToLineParam.SetValue<bool>(_GoToLineChecked);
            }
        }


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

        public GitDiffViewModel(IDbObjectText DbObjectText, IIDEProvider IDE, IGitAPI Git, IWarnings Warnings, ISettings Settings, IPluginSettingsStorage PluginSettingsStorage)
        {
            _IDEProvider = IDE;
            _DbObjectText = DbObjectText;
            _PluginSettingsStorage = PluginSettingsStorage;

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

            GoToLineChecked = Settings.GOTOLINE;

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

        public bool ClickOnFlowDocumnet(string line)
        {
            Seri.Log.Here().Debug($"line: {line}");
            if (_GoToLineChecked)
            {
                // Вытягиваем номер строки из правой колонки gitdiffline
                Regex regex = new Regex(@"^\d*\s+(?<val>\d+)");
                Match match = regex.Match(line, 0, Math.Min(line.Length, 16));
                if (match.Groups["val"].Success)
                {
                    _IDEProvider.GoToLine(int.Parse(match.Groups["val"].Value));
                    // Закрываем окно GitDiff
                    return true;
                }
            }
            return false;
        }
    }
}
