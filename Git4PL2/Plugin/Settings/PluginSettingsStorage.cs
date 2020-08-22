using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    /// Классы для параметров
    ///  - PluginParameter<string>      TextBox             Обычный текстовый параметр
    ///  - PluginParameter<bool>        CheckBox            Булевый параметр с двумя значения Да/Нет
    ///  - PluginParameterPath          TextBox + button    Текстовый параметр для хранения ссылки на папку. В окне настроек будет кнопка с выбором папки
    ///  - PluginParameterList          Combobox            Список. Необходимо передать тип Enum на основе которого создастся выпадающий список
    ///                                                     Значения enum должны быть с атрибутом Decription
    /// </summary>
    public class PluginSettingsStorage : IPluginSettingsStorage
    {
        private JObject _DefaultConfiguration;

        private List<IPluginParameter> _ListSettings;

        private List<PluginParameterGroup> _ListGroup;

        public IEnumerable<PluginParameterGroup> GetGroups => _ListGroup;

        public IPluginParameter GetParam(ePluginParameterNames name)
        {
            return _ListSettings.First(x => x.Name == name);
        }

        public T GetParamValue<T>(ePluginParameterNames name)
        {
            return _ListSettings.First(x => x.Name == name).GetValue<T>();
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
                Seri.Log.Here().Debug("Пробуем загрузить Json файл с найстройками по умолчанию");
                _DefaultConfiguration = JObject.Parse(File.ReadAllText("Git4PL2DefaultPreset.json"));
                Seri.Log.Here().Information("Json файл загружен");
            }
            catch (Exception ex)
            {
                Seri.LogException(ex);
                throw (ex);
            }

            FillGroups();
            FillSettings();

            Properties.Settings.Default.Save();
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

            _ListSettings.Add(new PluginParameterPath(ePluginParameterNames.GitRepositoryPath, @"D:\Repo")
            {
                Description = "Репозиторий Git",
                DescriptionExt = "Необходимо указать расположение репозитрия Git с объектами DB",
                Group = ePluginParameterGroupType.Main,
                OrderPosition = 1
            });

            _ListSettings.Add(new PluginParameterList(ePluginParameterNames.SaveEncodingType, 2, typeof(eSaveEncodingType))
            {
                Description = "Кодировка по умолчанию",
                DescriptionExt = @"Файл в репозитории Git может иметь маркер кодировки UTF8 – BOM. BOM - Byte Order Mark, Маркер последовательности " +
                "байтов или метка порядка – специальный символ из стандарта Юникод, вставляемый в начало текстового файла или потока для обозначения того, " +
                "что в файле(потоке) используется Юникод, а также для косвенного указания кодировки и порядка байтов, " +
                "с помощью которых символы Юникода были закодированы. Рекомендуется не менять существующий формат.",
                Group = ePluginParameterGroupType.Main,
                ParamterType = ePluginParameterUIType.List,
                OrderPosition = 100
            });

            #endregion


            #region GitDiff group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffAddSchema, true)
            {
                Description = "Добавлять префикс схемы к названию объекта",
                DescriptionExt = "Эта настройка, дополняет предыдущую. При отсутствии схемы в названии объекта БД, она будет добавлена. (Такое встречается в файлах Git)",
                Group = ePluginParameterGroupType.GitDiff,
                OrderPosition = 30
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffChangeCor, true)
            {
                Description = "Игнорировать изменения до названия объекта",
                DescriptionExt = "Касается переноса после ‘Create or replace’ а также лишних пробелов, которые могут встречаться в файле Git и " +
                "отсутствовать в PL/SQL Developer. При включенной настройке, вы не увидите этих изменений в первых строках файла.",
                Group = ePluginParameterGroupType.GitDiff,
                OrderPosition = 10
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffChangeName, true)
            {
                Description = "Сохранять название объекта, как в git",
                DescriptionExt = "Название объекта БД открытого в PL/SQL Developer может не всегда совпадать с названием того же объекта в Git. " +
                "Если опция включена, название объекта всегда будет соответствовать версии названия в файле в репозитории Git.",
                Group = ePluginParameterGroupType.GitDiff,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffCRLF, true)
            {
                Description = "Игн. изменения пробелов в конце строк",
                DescriptionExt = "В PL/SQL Developer применяется стандартный для Windows перенос строк в два символа CR-LF (“Carriage Return” и “Line Feed”). " +
                "Если в локальном файле репозитория Git, перенос строк реализован через один символ LF (пока такое встречается редко), то при включённой опции, " +
                "CR-LF в тексте объекта БД будет заменено на LF. Это позволит избежать конфликтов в каждой строчке текста. Так же эту проблему можно решить, " +
                "если в настрйоках Git установить переменную autocrlf в true(выполнить git config--global core.autocrlf true)",
                Group = ePluginParameterGroupType.GitDiff,
                OrderPosition = 50
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffWorkWithSlash, true)
            {
                Description = "Обрабатывать '/' в конце файла",
                DescriptionExt = "Текст в файлах в репозитории Git заканчивается на ‘/’. Обработка этого символа предотвращает его " +
                "затирание при операции сохранения текста. А также убирает этот символ при операции загрузки текста.",
                Group = ePluginParameterGroupType.GitDiff,
                ParamterType = ePluginParameterUIType.CheckBox,
                OrderPosition = 40
            });

            #endregion


            #region Warning group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.UnexpectedBranch, true)
            {
                Description = "Проверка ветки при сохранении текста в репозиторий",
                DescriptionExt = "При сохранении текста объекта в репозитории, если название ветки не совпадает с соответствующим регулярным выражением " +
                "– появится предупреждение, во избежание отправки изменений не в ту ветку.",
                Group = ePluginParameterGroupType.Warning,
                OrderPosition = 10
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.UnexpectedServer, true)
            {
                Description = "Проверка сервера при загрузки текста в PL/SQL Developer",
                DescriptionExt = "При загрузке текста в PL/SQL Developer, если название сервера не совпадает с соответствующим регулярным выражением – " +
                "появится предупреждение, во избежание изменения объекта БД не на том сервере",
                Group = ePluginParameterGroupType.Warning,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.WarnInRegEx, string.Empty)
            {
                Description = "Регулярное выражение, для проверки операции SaveText",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Warning,
                OrderPosition = 11
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.WarnOutRegEx, string.Empty)
            {
                Description = "Регулярное выражение, для проверки операции LoadText",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Warning,
                OrderPosition = 21
            });

            #endregion


            #region Blame Group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.ShowGitBlameProperties, true)
            {
                Description = "Запрашивать настройки для команды GitBlame",
                DescriptionExt = "Если опция включена, то перед операцией GitBlame можно будет выбрать кол-во строк которое будет обработано. Если же отклчить настройку, то по умолчанию обработаются только 10 строк",
                Group = ePluginParameterGroupType.Blame,
                OrderPosition = 10
            });

            var CommitViewURL = _DefaultConfiguration.SelectToken("CommitViewURL").ToString();
            if (string.IsNullOrEmpty(CommitViewURL))
                CommitViewURL = "https://www.google.com/search?q=";
            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.CommitViewURL, CommitViewURL)
            {
                Description = "URL для запуска информации по коммиту",
                DescriptionExt = "В окне GitBlame при запросе информации по комиту будет переход по этой ссылки. (К этой ссылке в конце будет добавлен sha комита)",
                Group = ePluginParameterGroupType.Blame,
                OrderPosition = 20
            });

            #endregion


            #region Others group

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.ClassicButtonsPosition, false)
            {
                Description = "Классическое расположение кнопок в окне Gitt Diff",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Others,
                OrderPosition = 10
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DICTI_CHILDREN_LIMIT_ENABLE, true)
            {
                Description = "Включить ограничение для отбора дочерних записей из Dicti",
                Group = ePluginParameterGroupType.Others,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<int>(ePluginParameterNames.DICTI_CHILDREN_LIMIT_VALUE, 20)
            {
                Description = "Лимит на кол0во отобранных дочерних записей для Dicti",
                Group = ePluginParameterGroupType.Others,
                OrderPosition = 20
            });

            #endregion


            #region SQL Group

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.SQL_DICTI_PARENT_COUNT,  _DefaultConfiguration.SelectToken("SQL_DICTI_PARENT_COUNT").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.SQL_DICTI_PARENT, _DefaultConfiguration.SelectToken("SQL_DICTI_PARENT").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.SQL_DICTI_HIERARCHY, _DefaultConfiguration.SelectToken("SQL_DICTI_HIERARCHY").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.SQL_DICTIISN_BY_CONSTNAME, _DefaultConfiguration.SelectToken("SQL_DICTIISN_BY_CONSTNAME").ToString())
            {
                Group = ePluginParameterGroupType.SQL
            });

            #endregion
        }
    }
}
