using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Settings
{
    /// <summary>
    /// По умолчанию в настройках приложения [Properties.Settings.Default] пусто!
    /// Все переменные приложения, будут:
    ///  - Инициализироваться здесь(Добавляться в Properties.Settings.Default в ручную при запуске плагина)
    ///  - При этом брать значение по умолчанию/Или из файла настроек
    ///  - Сохраняться по умолчанию в user.config как обычные праметры
    ///
    /// Это позволит организовано держать все имена переменных в одном месте: ePluginParameterNames, а так же удобно организоваьб окно настроек приложения.
    ///
    /// Что бы добавить новый параметр в приложение, нужно
    ///  1. Добавить название параметра в ePluginParameterNames
    ///  2. Добавить параметр в коллекцию ListSettings ниже здесь
    ///  3. Описать параметра в ISettings и использовать в приложении
    /// При этом параметр автоматически подтянется в окно настроек приложения
    /// </summary>
    public class PluginSettingsStorage : IPluginSettingsStorage
    {
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
            Seri.Log.Here().Information("Файл настроек: " + ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);

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
                GroupType = ePluginParameterGroupType.Others,
                Name = "Прочее"
            });

            _ListSettings = new List<IPluginParameter>();

            _ListSettings.Add(new PluginParameterPath(ePluginParameterNames.GitRepositoryPath, @"D:\Repo")
            {
                Description = "Репозиторий Git",
                DescriptionExt = "Необходимо указать расположение репозитрия Git с объектами DB",
                Group = ePluginParameterGroupType.Main,
                ParamterType = ePluginParameterType.Path,
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
                ParamterType = ePluginParameterType.List,
                OrderPosition = 100
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffAddSchema, true)
            {
                Description = "Добавлять префикс схемы к названию объекта",
                DescriptionExt = "Эта настройка, дополняет предыдущую. При отсутствии схемы в названии объекта БД, она будет добавлена. (Такое встречается в файлах Git)",
                Group = ePluginParameterGroupType.GitDiff,
                ParamterType = ePluginParameterType.CheckBox,
                OrderPosition = 30
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffChangeCor, true)
            {
                Description = "Игнорировать изменения до названия объекта",
                DescriptionExt = "Касается переноса после ‘Create or replace’ а также лишних пробелов, которые могут встречаться в файле Git и " +
                "отсутствовать в PL/SQL Developer. При включенной настройке, вы не увидите этих изменений в первых строках файла.",
                Group = ePluginParameterGroupType.GitDiff,
                ParamterType = ePluginParameterType.CheckBox,
                OrderPosition = 10
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffChangeName, true)
            {
                Description = "Сохранять название объекта, как в git",
                DescriptionExt = "Название объекта БД открытого в PL/SQL Developer может не всегда совпадать с названием того же объекта в Git. " +
                "Если опция включена, название объекта всегда будет соответствовать версии названия в файле в репозитории Git.",
                Group = ePluginParameterGroupType.GitDiff,
                ParamterType = ePluginParameterType.CheckBox,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffCRLF, true)
            {
                Description = "Игн. изменения пробелов в конце строк",
                DescriptionExt = "В PL/SQL Developer применяется стандартный для Windows перенос строк в два символа CR-LF (“Carriage Return” и “Line Feed”). " +
                "Если в локальном файле репозитория Git, перенос строк реализован через один символ LF (пока такое встречается редко), то при включённой опции, " +
                "CR-LF в тексте объекта БД будет заменено на LF. Это позволит избежать конфликтов в каждой строчке текста. Так же эту проблему можно решить, "+
                "если в настрйоках Git установить переменную autocrlf в true(выполнить git config--global core.autocrlf true)",
                Group = ePluginParameterGroupType.GitDiff,
                ParamterType = ePluginParameterType.CheckBox,
                OrderPosition = 50
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.DiffWorkWithSlash, true)
            {
                Description = "Обрабатывать '/' в конце файла",
                DescriptionExt = "Текст в файлах в репозитории Git заканчивается на ‘/’. Обработка этого символа предотвращает его " +
                "затирание при операции сохранения текста. А также убирает этот символ при операции загрузки текста.",
                Group = ePluginParameterGroupType.GitDiff,
                ParamterType = ePluginParameterType.CheckBox,
                OrderPosition = 40
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.UnexpectedBranch, true)
            {
                Description = "Проверка ветки при сохранении текста в репозиторий",
                DescriptionExt = "При сохранении текста объекта в репозитории, если название ветки не совпадает с соответствующим регулярным выражением " +
                "– появится предупреждение, во избежание отправки изменений не в ту ветку.",
                Group = ePluginParameterGroupType.Warning,
                ParamterType = ePluginParameterType.CheckBox,
                OrderPosition = 10
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.UnexpectedServer, true)
            {
                Description = "Проверка сервера при загрузки текста в PL/SQL Developer",
                DescriptionExt = "При загрузке текста в PL/SQL Developer, если название сервера не совпадает с соответствующим регулярным выражением – " +
                "появится предупреждение, во избежание изменения объекта БД не на том сервере",
                Group = ePluginParameterGroupType.Warning,
                ParamterType = ePluginParameterType.CheckBox,
                OrderPosition = 20
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.WarnInRegEx, string.Empty)
            {
                Description = "Регулярное выражение, для проверки операции SaveText",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Warning,
                ParamterType = ePluginParameterType.Text,
                OrderPosition = 11
            });

            _ListSettings.Add(new PluginParameter<string>(ePluginParameterNames.WarnOutRegEx, string.Empty)
            {
                Description = "Регулярное выражение, для проверки операции LoadText",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Warning,
                ParamterType = ePluginParameterType.Text,
                OrderPosition = 21
            });

            _ListSettings.Add(new PluginParameter<bool>(ePluginParameterNames.ClassicButtonsPosition, false)
            {
                Description = "Классическое расположение кнопок в окне Gitt Diff",
                DescriptionExt = string.Empty,
                Group = ePluginParameterGroupType.Others,
                ParamterType = ePluginParameterType.CheckBox,
                OrderPosition = 10
            });

            Properties.Settings.Default.Save();
        }
    }
}
