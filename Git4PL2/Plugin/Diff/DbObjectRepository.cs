using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.Diff
{
    class DbObjectRepository : DbObject, IDbObjectRepository
    {
        protected ISettings _settings;
      
        public string FileName { get; private set; }
        public eDbObjectType ObjectTypeItem { get; private set; }
        public string FileExtension { get; private set; }

        public string DescriptionName => string.Join(" ", ObjectTypeItem.ToString(), string.Join(".", ObjectOwner, ObjectName)).ToLower();
        public string RepName => Path.Combine(ObjectOwner, FileName);

        /// <summary>
        /// Путь расположения файла в локальном репозитории git
        /// RAW - так как не соблюдён CaseSensitive
        /// </summary>
        public string GetRawFilePath()
        {
            string GitRep = _settings.GitRepositoryPath;
            if (Directory.Exists(GitRep))
                return Path.Combine(GitRep, ObjectOwner, string.Join(".", ObjectOwner, ObjectName, FileExtension)).ToLower();
            else
                throw new Exception("Не указан локальный репозиторий GIT");
        }

        /// <summary>
        /// Путь расположения папки где должен находиться файл
        /// RAW - так как не соблюдён CaseSensitive
        /// </summary>
        public string GetRawDirPath()
        {
            string GitRep = _settings.GitRepositoryPath;
            if (Directory.Exists(GitRep))
                return Path.Combine(GitRep, ObjectOwner);
            else
                throw new Exception("Не указан локальный репозиторий GIT");
        }

        public string ObjectTypeToString => ObjectTypeItem.ToString();

        public DbObjectRepository(string Owner, string name, string type) :base( Owner, name, type)
        {
            Seri.Log.Here().Debug($"Создаём объект DbObject. Owner={Owner}, name={name}, type={type}");

            _settings = NinjectCore.Get<ISettings>();

            if (string.IsNullOrWhiteSpace(Owner) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type))
                throw new Exception("Не удалось распознать объект БД");

            if (Directory.Exists(GetRawDirPath()))
                this.ObjectOwner = Helper.GetCaseSensitiveFolderName(GetRawDirPath());

            try
            {
                ObjectTypeItem = (eDbObjectType)Enum.Parse(typeof(eDbObjectType), type.Replace(" ", "").ToUpper(), true);
            }
            catch
            {
                throw new Exception($"Тип объекта: {type} - не поддерживается");
            }

            FileExtension = Helper.FileExtension[ObjectTypeItem];
            Seri.Log.Here().Verbose("FileExtension={0}", FileExtension);

            if (File.Exists(GetRawFilePath()))
                FileName = Helper.GetCaseSensitiveFileName(GetRawFilePath());

            Seri.Log.Here().Verbose($"Конец конструктора DbObject");
        }

        public DbObjectRepository(DbObjectRepository obj): base(obj.ObjectOwner, obj.ObjectName, obj.ObjectType)
        {
            _settings = obj._settings;
            FileName = obj.FileName;
            ObjectTypeItem = obj.ObjectTypeItem;
            FileExtension = obj.FileExtension;
        }

        public void DirectoriesChecks()
        {
            if (!Directory.Exists(_settings.GitRepositoryPath))
                throw new Exception("Не указан локальный репозиторий GIT");

            string DirPath = GetRawDirPath();
            if (!Directory.Exists(DirPath))
            {
#if DEBUG 
                MessageBoxResult dialogResult = MessageBoxResult.Yes;
#else
                MessageBoxResult dialogResult = MessageBox.Show("Не найдена директория " + DirPath + ". Хотите создать?", "Отсутствует директория для схемы", MessageBoxButton.YesNo, MessageBoxImage.Warning);
#endif
                if (dialogResult == MessageBoxResult.Yes)
                    Directory.CreateDirectory(DirPath);
                else
                    throw new Exception($"В локальном репозитории GIT отсутствует директория для объекта БД({DirPath})");
            }

            if (!File.Exists(GetRawFilePath()))
            {
#if DEBUG 
                MessageBoxResult dialogResult = MessageBoxResult.Yes;
#else
                MessageBoxResult dialogResult = MessageBox.Show("Не найден файл " + GetRawFilePath() + ". Хотите создать его?", "Файл отсутствует в локальном репозитории", MessageBoxButton.YesNo, MessageBoxImage.Warning);
#endif
                if (dialogResult == MessageBoxResult.Yes)
                    File.WriteAllText(GetRawFilePath().ToUpper(), "\r\n/");
                else
                    throw new Exception($"В локальном репозитории GIT отсутствует файл для сохранения объекта БД ({GetRawFilePath()})");
            }
        }
    }
}
