using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Model;
using Git4PL2.Plugin.WPF.Model;
using Newtonsoft.Json;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.TeamCoding.FileProvider
{
    class TeamCodingFileProvider : ITeamCodingProvider
    {
        private const string FILE_NAME = "Git4PL2_TeamCoding.json";
        private readonly string FILE_PATH;
        private ITeamCodingProviderChecks _TeamCodingChecks;
        private ISettings _Settings;
        private string _ServerName;

        public TeamCodingFileProvider(ITeamCodingProviderChecks TeamCodingChecks, ISettings Settings, IIDEProvider IDEProvider)
        {
            Seri.Log.Here().Debug("TeamCodingFileProvider ctor");

            _TeamCodingChecks = TeamCodingChecks;
            _Settings = Settings;

            var path = Settings.TEAMCODING_FILEPROVIDER_PATH;
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                throw new Exception("Не указано расположение файла TeamCoding");
            }

            FILE_PATH = Path.Combine(path, FILE_NAME);
            _ServerName = IDEProvider.SQLQueryExecute<DummyString>(Settings.SQL_SERVERNAME)[0].Value;
        }

        public IEnumerable<ICheckOutObject> GetCheckOutObjectsList()
        {
            return ReadFile().CheckOutObjectsList;
        }


        public bool RestrickCompileWithoutCheckOut
        {
            get
            {
                return ReadFile().RestrickCompileWithoutCheckOut;
            }
            set
            {
                TeamCodingFile file = ReadFile();
                file.RestrickCompileWithoutCheckOut = value;
                SaveFile(file);
            }
        }

        public bool CheckIn(string Login, IDbObject dbObject, out string ErrorMsg)
        {
            TeamCodingFile file = ReadFile();
            CheckSettings(file);
            CheckOutObject CheckInObject = new CheckOutObject()
            {
                Login = _Settings.TEAMCODING_LOGIN,
                ServerName = _ServerName,
                ObjectOwner = dbObject.ObjectOwner,
                ObjectName = dbObject.ObjectName,
                ObjectType = dbObject.ObjectType
            };

            if (!_TeamCodingChecks.ChecksBeforeCheckIn(file.CheckOutObjectsList, CheckInObject, out ErrorMsg))
                return false;


            file.CheckOutObjectsList = file.CheckOutObjectsList.Where(x => !x.Equals(CheckInObject)).ToEnumerable();
            SaveFile(file);
            return true;
        }

        public bool CheckOut(string Login, IDbObject dbObject, out string ErrorMsg)
        {
            TeamCodingFile file = ReadFile();
            CheckSettings(file);
            CheckOutObject CheckOutbject = new CheckOutObject()
            {
                Login = _Settings.TEAMCODING_LOGIN,
                ServerName = _ServerName,
                ObjectOwner = dbObject.ObjectOwner,
                ObjectName = dbObject.ObjectName,
                ObjectType = dbObject.ObjectType,
                CheckoutDate = DateTime.Now
            };

            if (!_TeamCodingChecks.ChecksBeforeCheckOut(file.CheckOutObjectsList, CheckOutbject, out ErrorMsg))
                return false;

            var list = file.CheckOutObjectsList.ToArray();
            var listLen = list.Length;

            Array.Resize(ref list, listLen + 1);
            list[listLen] = CheckOutbject;

            file.CheckOutObjectsList = list.ToEnumerable();
            SaveFile(file);
            return true;
        }

        public string GetUserOwner(IDbObject dbObject, string server)
        {
            var CheckOutObject = ReadFile().CheckOutObjectsList.Where(x => x.ServerName == server
                                                        && x.ObjectName == dbObject.ObjectName
                                                        && x.ObjectOwner == dbObject.ObjectOwner
                                                        && x.ObjectType == dbObject.ObjectType);
            if (CheckOutObject.Any())
                return CheckOutObject.First().Login;
            else
                return string.Empty;
        }

        private TeamCodingFile ReadFile()
        {
            if (!File.Exists(FILE_PATH))
                return new TeamCodingFile() { RestrickCompileWithoutCheckOut = false, CheckOutObjectsList = new CheckOutObject[] { } };
            string FileText = File.ReadAllText(FILE_PATH);
            return JsonConvert.DeserializeObject<TeamCodingFile>(FileText);
        }

        private void SaveFile(TeamCodingFile file)
        {
            var json = JsonConvert.SerializeObject(file);
            File.WriteAllText(FILE_PATH, json);
        }

        private void CheckSettings(TeamCodingFile file)
        {
            if (_Settings.TEAMCODING_RESTRICT_COMPILE_WITHOUT_CHECKOUT != file.RestrickCompileWithoutCheckOut)
            {
                var storage = NinjectCore.Get<IPluginSettingsStorage>();
                var parameter = storage.GetParam(Settings.ePluginParameterID.TEAMCODING_RESTRICT_COMPILE_WITHOUT_CHECKOUT);
                parameter.SetValue<bool>(file.RestrickCompileWithoutCheckOut);

                MessageBox.Show($"В TeamCoding зафиксировано изменение настройки RestrickCompileWithoutCheckOut. " +
                    $"Установлено значение [{file.RestrickCompileWithoutCheckOut}]", "Обновление настрйоки", MessageBoxButton.OK, MessageBoxImage.Warning);
            }    
        }
    }
}
