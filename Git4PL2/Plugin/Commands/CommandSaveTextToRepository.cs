using Git4PL2.Abstarct;
using Git4PL2.Git.Abstract;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    class CommandSaveTextToRepository : PluginCommand
    {
        private bool _CanExecute = true;

        private IIDEProvider _IDEProvider;
        private IDbObjectText _DbObjectText;
        private IGitAPI _GitAPI;
        private IWarnings _Warnings;
        private string _BranchName;


        public CommandSaveTextToRepository(IIDEProvider IDEProvider, IGitAPI GitAPI, IWarnings Warnings):base("SaveTextToRepository")
        {
            _IDEProvider = IDEProvider;
            _GitAPI = GitAPI;
            _Warnings = Warnings;
        }

        public override void Execute(object parameter)
        {
            _CanExecute = false;
            try
            {
                if (parameter != null && parameter is TextOperationsParametrs TextParam)
                {
                    _DbObjectText = TextParam.DbObjectText;
                    _BranchName = TextParam.StringParam;
                }
                SaveText();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _CanExecute = true;
            }
        }

        private void SaveText()
        {
            if (_DbObjectText == null)
                _DbObjectText = _IDEProvider.GetDbObject<IDbObjectText>();
            else
                _DbObjectText.DirectoriesChecks();

            if (_BranchName == null)
                _BranchName = _GitAPI.GetCurrentBranch();

            if (_Warnings.IsBranchUnexsepted(_BranchName))
                return;

            string FilePath = _DbObjectText.GetRawFilePath();
            Seri.Log.Here().Debug("FilePath={0}", FilePath);

            File.WriteAllText(FilePath, _DbObjectText.Text, _DbObjectText.GetSaveEncoding());
            _IDEProvider.SetStatusMessage($"Объект БД сохранён в: {FilePath}");
        }

        public override bool CanExecute(object parameter)
        {
            return _CanExecute;
        }
    }
}
