using Git4PL2.Abstarct;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using Git4PL2.Plugin.TeamCoding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.Settings
{
    /// <summary>
    /// 
    /// Настройки через Configuration не получилось запустить, в девелопере вылезала ошибка:
    /// https://github.com/dotnet/extensions/issues/2931
    /// https://stackoverflow.com/questions/43995432/could-not-load-file-or-assembly-microsoft-extensions-dependencyinjection-abstrac
    /// 
    /// Сохранять и загружать настройки приложения будем через стандартный механизм net.framework (user.config)
    /// В Git4PL2DefaultPreset.json будут лежать "приватные" значения по умолчанию для части настроек.
    /// 
    /// По умолчанию в настройках приложения [Properties.Settings.Default] пусто!
    /// Все переменные приложения, будут:
    ///  - Инициализироваться здесь(Добавляться в Properties.Settings.Default в ручную при запуске плагина)
    ///  - Если значение еще не было изменено пользователем оно возьмется по умолчанию:
    ///      * Из хардкода ниже 
    ///     или
    ///      * Из Git4PL2DefaultPreset.json
    ///  - Если значение было изменено пользователем, то оно подтянется из файла настроек user.config
    ///  - Соответственно, когда пользователь меняет значение оно обновляется в user.config (и только там)  
    ///
    /// Это позволит организовано держать все имена переменных в одном месте: ePluginParameterNames, а так же удобно организовывать окно настроек приложения.
    ///
    /// Что бы добавить новый параметр в приложение, нужно
    ///  1. Добавить название параметра в ePluginParameterNames
    ///  2. Добавить параметр в коллекцию ListSettings ниже здесь
    ///  3. Описать параметра в ISettings и использовать в приложении
    /// При этом параметр автоматически подтянется в окно настроек приложения
    /// 
    ///  |Тип UI    | Класс параметра             | Контент           | Описание                      
    ///  ---------------------------------------------------------------------------------------------------------------------------------------------
    ///  | Text     | PluginParameter<string>     | TextBox           | Обычный текстовый параметр
    ///  | Number   | PluginParameter<int>        | TextBox           | Обычный текстовый параметр с числовым значением. Любое другое значение не будет сохранено
    ///  | CheckBox | PluginParameter<bool>       | CheckBox          | Булевый параметр с двумя значения Да/Нет
    ///  | Path     | PluginParameter<string>     | TextBox + button  | Текстовый параметр для хранения ссылки на папку. В окне настроек будет кнопка с выбором папки
    ///  | List     | PluginParameterList         | Combobox          | Список. Необходимо передать тип Enum на основе которого создастся выпадающий список
    ///  |          |                             |                   | Значения enum должны быть с атрибутом Decription
    /// </summary>
    public class PluginSettingsStorage : IPluginSettingsStorage
    {
        private JObject _DefaultConfiguration;

        private List<IPluginParameter> _ListSettings;

        private List<PluginParameterGroup> _ListGroup;

        public IEnumerable<PluginParameterGroup> GetGroups => _ListGroup;

        public IPluginParameter GetParam(ePluginParameterID name)
        {
            return _ListSettings.First(x => x.ID == name);
        }

        public T GetParamValue<T>(ePluginParameterID name)
        {
            return _ListSettings.First(x => x.ID == name).GetValue<T>();
        }

        public IEnumerable<IPluginParameter> ParametersByGroup(ePluginParameterGroupType GroupType)
        {
            return _ListSettings.Where(x => x.Group == GroupType).OrderBy(x => x.OrderPosition);
        }

        public PluginSettingsStorage()
        {
            Seri.Log.Here().Information("Инициализируем настройки приложения");
            Seri.Log.Here().Information("Расположение файл настроек: " + ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);

            try
            {
                var JsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Git4PL2DefaultPreset.json");
                Seri.Log.Here().Debug("Пробуем загрузить Json файл с найстройками по умолчанию " + JsonPath);
                _DefaultConfiguration = JObject.Parse(File.ReadAllText(JsonPath));
                Seri.Log.Here().Information("Json файл загружен");
            }
            catch (Exception ex)
            {
                Seri.LogException(ex);
                throw ex;
            }

            FillGroups();
            FillSettings();

            Properties.Settings.Default.Save();

            NinjectCore.SetTeamCodingProvider((eTeamCodingProviderType)GetParamValue<int>(ePluginParameterID.TEAMCODING_PROVIDER));
        }

        private void FillGroups()
        {
            _ListGroup = new List<PluginParameterGroup>();

            _ListGroup.Add(new PluginParameterGroup()
            {
                GroupType = ePluginParameterGroupType.Main,
                Name = "Основные"
            });

            _ListGroup.Add(new PluginParameterGroup()
            {
                GroupType = ePluginParameterGroupType.GitDiff,
                Name = "GitDiff"
            });

            _ListGroup.Add(new PluginParameterGroup()
            {
                GroupType = ePluginParameterGroupType.TeamCoding,
                Name = "Team Coding"
            });

            _ListGroup.Add(new PluginParameterGroup()
            {
                GroupType = ePluginParameterGroupType.Warning,
                Name = "Предупреждения"
            });

            _ListGroup.Add(new PluginParameterGroup()
            {
                GroupType = ePluginParameterGroupType.Blame,
                Name = "Настройки GitBlame"
            });

            _ListGroup.Add(new PluginParameterGroup()
            {
                GroupType = ePluginParameterGroupType.Others,
                Name = "Прочее"
            });
        }

        private void FillSettings()
        {
            _ListSettings = new List<IPluginParameter>();

            #region Main group

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.GitRepositoryPath, @"D:\Repo")
            {
                Description = "Репозиторий Git",
                DescriptionExt = "Необходимо указать расположение репозитрия Git с объектами DB",
                Group = ePluginParameterGroupType.Main,
                ParamterUIType = ePluginParameterUIType.Path,
                OrderPosition = 1
            });

            _ListSettings.Add(new PluginParameterList(ePluginParameterID.SaveEncodingType, 2, typeof(eSaveEncodingType))
            {
                Description = "Кодировка по умолчанию",
                DescriptionExt = @"Файл в репозитории Git может иметь маркер кодировки UTF8 – BOM. BOM - Byte Order Mark, Маркер последовательности " +
                "байтов или метка порядка – специальный символ из стандарта Юникод, вставляемый в начало текстового файла или потока для обозначения того, " +
                "что в файле(потоке) используется Юникод, а также для косвенного указания кодировки и порядка байтов, " +
                "с помощью которых символы Юникода были закодированы. Рекомендуется не менять существующий формат.",
                Group = ePluginParameterGroupType.Main,
                ParamterUIType = ePluginParameterUIType.List,
                OrderPosition = 100
            });

            #endregion


            #region GitDiff group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.DiffAddSchema, true)
            {
                Description = "Добавлять префикс схемы к названию объекта",
                DescriptionExt = "Эта настройка, дополняет предыдущую. При отсутствии схемы в названии объекта БД, она будет добавлена. (Такое встречается в файлах Git)",
                Group = ePluginParameterGroupType.GitDiff,
                OrderPosition = 30
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.DiffChangeCor, true)
            {
                Description = "Игнорировать изменения до названия объекта",
                DescriptionExt = "Касается переноса после ‘Create or replace’ а также лишних пробелов, которые могут встречаться в файле Git и " +
                "отсутствовать в PL/SQL Developer. При включенной настройке, вы не увидите этих изменений в первых строках файла.",
                Group = ePluginParameterGroupType.GitDiff,
                OrderPosition = 10
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.DiffChangeName, true)
            {
                Description = "Сохранять название объекта, как в git",
                DescriptionExt = "Название объекта БД открытого в PL/SQL Developer может не всегда совпадать с названием того же объекта в Git. " +
                "Если опция включена, название объекта всегда будет соответствовать версии названия в файле в репозитории Git.",
                Group = ePluginParameterGroupType.GitDiff,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.DiffCRLF, true)
            {
                Description = "Игн. изменения пробелов в конце строк",
                DescriptionExt = "В PL/SQL Developer применяется стандартный для Windows перенос строк в два символа CR-LF (“Carriage Return” и “Line Feed”). " +
                "Если в локальном файле репозитория Git, перенос строк реализован через один символ LF (пока такое встречается редко), то при включённой опции, " +
                "CR-LF в тексте объекта БД будет заменено на LF. Это позволит избежать конфликтов в каждой строчке текста. Так же эту проблему можно решить, " +
                "если в настрйоках Git установить переменную autocrlf в true(выполнить git config--global core.autocrlf true)",
                Group = ePluginParameterGroupType.GitDiff,
                OrderPosition = 50
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.DiffWorkWithSlash, true)
            {
                Description = "Обрабатывать '/' в конце файла",
                DescriptionExt = "Текст в файлах в репозитории Git заканчивается на ‘/’. Обработка этого символа предотвращает его " +
                "затирание при операции сохранения текста. А также убирает этот символ при операции загрузки текста.",
                Group = ePluginParameterGroupType.GitDiff,
                ParamterUIType = ePluginParameterUIType.CheckBox,
                OrderPosition = 40
            });

            #endregion


            #region Warning group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.UnexpectedBranch, true)
            {
                Description = "Проверка ветки при сохранении текста в репозиторий",
                DescriptionExt = "При сохранении текста объекта в репозитории, если название ветки не совпадает с соответствующим регулярным выражением " +
                "– появится предупреждение, во избежание отправки изменений не в ту ветку.",
                Group = ePluginParameterGroupType.Warning,
                OrderPosition = 10
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.UnexpectedServer, true)
            {
                Description = "Проверка сервера при загрузки текста в PL/SQL Developer",
                DescriptionExt = "При загрузке текста в PL/SQL Developer, если название сервера не совпадает с соответствующим регулярным выражением – " +
                "появится предупреждение, во избежание изменения объекта БД не на том сервере",
                Group = ePluginParameterGroupType.Warning,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.WarnInRegEx, string.Empty)
            {
                Description = "Регулярное выражение, для проверки операции SaveText",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Warning,
                ParentParameterID = ePluginParameterID.UnexpectedBranch,
                OrderPosition = 11
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.WarnOutRegEx, string.Empty)
            {
                Description = "Регулярное выражение, для проверки операции LoadText",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Warning,
                ParentParameterID = ePluginParameterID.UnexpectedServer,
                OrderPosition = 21
            });

            #endregion


            #region Blame Group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.ShowGitBlameProperties, true)
            {
                Description = "Запрашивать настройки для команды GitBlame",
                DescriptionExt = "Если опция включена, то перед операцией GitBlame можно будет выбрать кол-во строк которое будет обработано. Если же отклчить настройку, то по умолчанию обработаются только 10 строк",
                Group = ePluginParameterGroupType.Blame,
                OrderPosition = 10
            });

            var CommitViewURL = _DefaultConfiguration.SelectToken("CommitViewURL").ToString();
            if (string.IsNullOrEmpty(CommitViewURL))
                CommitViewURL = "https://www.google.com/search?q=";
            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.CommitViewURL, CommitViewURL)
            {
                Description = "URL для запуска информации по коммиту",
                DescriptionExt = "В окне GitBlame при запросе информации по комиту будет переход по этой ссылки. (К этой ссылке в конце будет добавлен sha комита)",
                Group = ePluginParameterGroupType.Blame,
                OrderPosition = 20
            });

            #endregion


            #region Others group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.ClassicButtonsPosition, false)
            {
                Description = "Классическое расположение кнопок в окне Gitt Diff",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Others,
                OrderPosition = 10
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.DICTI_CHILDREN_LIMIT_ENABLE, true)
            {
                Description = "Включить ограничение для отбора дочерних записей из Dicti",
                Group = ePluginParameterGroupType.Others,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<int>(ePluginParameterID.DICTI_CHILDREN_LIMIT_VALUE, 20)
            {
                Description = "Лимит на кол0во отобранных дочерних записей для Dicti",
                Group = ePluginParameterGroupType.Others,
                ParentParameterID = ePluginParameterID.DICTI_CHILDREN_LIMIT_ENABLE,
                OrderPosition = 20
            });

            #endregion


            #region SQL Group

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.SQL_DICTI_PARENT_COUNT, _DefaultConfiguration.SelectToken("SQL_DICTI_PARENT_COUNT").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.SQL_DICTI_PARENT, _DefaultConfiguration.SelectToken("SQL_DICTI_PARENT").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.SQL_DICTI_HIERARCHY, _DefaultConfiguration.SelectToken("SQL_DICTI_HIERARCHY").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.SQL_DICTIISN_BY_CONSTNAME, _DefaultConfiguration.SelectToken("SQL_DICTIISN_BY_CONSTNAME").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.SQL_SERVERNAME, _DefaultConfiguration.SelectToken("SQL_SERVERNAME").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.SQL_FTOGGLE, _DefaultConfiguration.SelectToken("SQL_FTOGGLE").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });


            #endregion


            #region TeamCoding Group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.TEAMCODING_ENABLE, false)
            {
                Description = "Вкл. TeamCoding",
                DescriptionExt = "TeamCoding позволяет команде работать на одном сервере. Добавляет возможность делать CheckOut, CheckIn для объектов",
                Group = ePluginParameterGroupType.TeamCoding,
                OrderPosition = 10,
                OnParameterChanged = (x) =>
                {
                    var menu = NinjectCore.Get<IMenu>();
                    menu.RefreshMenu();
                }
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.TEAMCODING_LOGIN, string.Empty)
            {
                Description = "Логин",
                DescriptionExt = "Ваше имя под которым объекты БД будут браться в пользование",
                Group = ePluginParameterGroupType.TeamCoding,
                ParentParameterID = ePluginParameterID.TEAMCODING_ENABLE,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterID.TEAMCODING_RESTRICT_COMPILE_WITHOUT_CHECKOUT, false) 
            {
                Description = "Запретить компиляцию без CheckOut",
                DescriptionExt = "Если объект в настоящий момет никем не занят, можно разрешить его компилировать без CheckOut. " +
                                "Эта настройка общая, изменения распространится на всех участников TEAMCODING. " +
                                "Пожалуйста договоритесь с командой о политиках работы на общей среде прежде чем менять настрйоку",
                Group = ePluginParameterGroupType.TeamCoding,
                ParentParameterID = ePluginParameterID.TEAMCODING_ENABLE,
                OrderPosition = 25,
                OnParameterChanged = (x) =>
                {
                    ITeamCodingProvider provider = NinjectCore.Get<ITeamCodingProvider>();
                    provider.RestrickCompileWithoutCheckOut = x;
                }
            });

            _ListSettings.Add(new PluginParameterList(ePluginParameterID.TEAMCODING_PROVIDER, 0, typeof(eTeamCodingProviderType))
            {
                Description = "Провайдер для TeamCoding-а",
                DescriptionExt = "Вариант обмена информацией между несколькими пользователями PL/SQL Developer. Для того что-бы команды была в одной \"сети\", " +
                                 "У всех должны быть одинаковые настройки.\n" +
                                 "Общий файл на диске - файл размешенный в сетевой папке к которому есть доступ у каждого участника. " +
                                 "При операции CheckOut в этот файл добавится информация, что пользователь <Логин> взял в пользование такой объект БД. " +
                                 "Другие участники при обращении к этому объекту БД, будут видеть предупреждение, что объект в настоящий момент занят.",
                Group = ePluginParameterGroupType.TeamCoding,
                ParentParameterID = ePluginParameterID.TEAMCODING_ENABLE,
                OrderPosition = 30,
                OnParameterChanged = (x) => NinjectCore.SetTeamCodingProvider((eTeamCodingProviderType)x)
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.TEAMCODING_FILEPROVIDER_PATH, string.Empty)
            {
                Description = "Путь для общего файла",
                ParamterUIType = ePluginParameterUIType.Path,
                Group = ePluginParameterGroupType.TeamCoding,
                ParentParameterID = ePluginParameterID.TEAMCODING_PROVIDER,
                ParentParameterStringValue = "0",
                OrderPosition = 40
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterID.TEAMCODING_SERVERNAME_REGEX, string.Empty)
            {
                Description = "Название рабочего сервера",
                DescriptionExt = "Регулярное выражение для имени сервера, который должен быть подключен к TeamCoding",
                Group = ePluginParameterGroupType.TeamCoding,
                ParentParameterID = ePluginParameterID.TEAMCODING_ENABLE,
                OrderPosition = 50
            });

            #endregion
        }
    }
}
