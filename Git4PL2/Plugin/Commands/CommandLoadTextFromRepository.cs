using Git4PL2.Abstarct;
using Git4PL2.Git.Abstract;
using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Commands
{
    class CommandLoadTextFromRepository : PluginCommand
    {
        private static bool _CanExecute = true;

        private IIDEProvider _IDEProvider;
        private IDbObjectText _DbObjectText;
        private IPlsqlCodeFormatter _PlsqlCodeFormatter;
        private IWarnings _Warnings;
        private string _ServerName;

        public CommandLoadTextFromRepository(IIDEProvider IDEProvider, IPlsqlCodeFormatter PlsqlCodeFormatter, IWarnings Warnings) :base("LoadTextFromRepository")
        {
            _IDEProvider = IDEProvider;
            _PlsqlCodeFormatter = PlsqlCodeFormatter;
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
                    _ServerName = TextParam.StringParam;
                }
                LoadText();
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

        private void LoadText()
        {
            if (_DbObjectText == null)
                _DbObjectText = _IDEProvider.GetDbObject<IDbObjectText>();
            else
                _DbObjectText.DirectoriesChecks();

            if (_ServerName == null)
                _ServerName = _IDEProvider.GetDatabaseConnection();

            if (_Warnings.IsServerUnexsepted(_ServerName))
                return;

            string FilePath = _DbObjectText.GetRawFilePath();
            Seri.Log.Here().Debug("FilePath={0}", FilePath);

            string LocalText = File.ReadAllText(FilePath);

            _PlsqlCodeFormatter.RemoveSlash(ref LocalText);

            _IDEProvider.SetText(LocalText);
            _IDEProvider.SetStatusMessage($"Объект БД загружен из: {FilePath}");
        }

        public override bool CanExecute(object parameter)
        {
            return _CanExecute;
        }
    }
}
