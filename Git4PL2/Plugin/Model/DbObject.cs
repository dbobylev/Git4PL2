using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.Plugin.Model
{
    class DbObject :IDbObject
    {
        public string ObjectOwner { get; private set; }
        public string ObjectName { get; private set; }
        public string FileName { get; private set; }
        public eDbObjectType ObjectType { get; private set; }
        public string FileExtension { get; private set; }

        public string DescriptionName => string.Join(" ", ObjectType.ToString(), string.Join(".", ObjectOwner, ObjectName)).ToLower();
        public string RepName => string.Join("/", ObjectOwner, FileName);

        /// <summary>
        /// Путь расположения файла в локальном репозитории git
        /// RAW - так как не соблюдён CaseSensitive
        /// </summary>
        public string GetRawFilePath()
        {
            string GitRep = Properties.Settings.Default.GitRepositoryPath;
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
            string GitRep = Properties.Settings.Default.GitRepositoryPath;
            if (Directory.Exists(GitRep))
                return Path.Combine(GitRep, ObjectOwner);
            else
                throw new Exception("Не указан локальный репозиторий GIT");
        }

        public string ObjectTypeToString => ObjectType.ToString();

        public DbObject(string Owner, string name, string type)
        {
            Seri.Log.Here().Debug($"Создаём объект DbObject. Owner={Owner}, name={name}, type={type}");

            if (string.IsNullOrWhiteSpace(Owner) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(type))
                throw new Exception("Не удалось распознать объект БД");

            this.ObjectOwner = Owner.ToUpper();
            if (Directory.Exists(GetRawDirPath()))
                this.ObjectOwner = Helper.GetCaseSensitiveFolderName(GetRawDirPath());

            ObjectName = name.ToUpper();

            try
            {
                ObjectType = (eDbObjectType)Enum.Parse(typeof(eDbObjectType), type.Replace(" ", "").ToUpper(), true);
            }
            catch
            {
                throw new Exception($"Тип объекта: {type} - не поддерживается");
            }

            FileExtension = Helper.FileExtension[ObjectType];
            Seri.Log.Here().Verbose("FileExtension={0}", FileExtension);

            if (File.Exists(GetRawFilePath()))
                FileName = Helper.GetCaseSensitiveFileName(GetRawFilePath());

            Seri.Log.Here().Verbose($"Конец конструктора DbObject");
        }

        public DbObject(DbObject obj)
        {
            ObjectOwner = obj.ObjectOwner;
            ObjectName = obj.ObjectName;
            ObjectType = obj.ObjectType;
            FileExtension = obj.FileExtension;
        }

        public void DirectoriesChecks()
        {
            if (!Directory.Exists(Properties.Settings.Default.GitRepositoryPath))
                throw new Exception("Не указан локальный репозиторий GIT");

            string DirPath = GetRawDirPath();
            if (!Directory.Exists(DirPath))
            {
                MessageBoxResult dialogResult = MessageBox.Show("Не найдена директория " + DirPath + ". Хотите создать?", "Отсутствует директория для схемы", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (dialogResult == MessageBoxResult.Yes)
                    Directory.CreateDirectory(DirPath);
                else
                    throw new Exception($"В локальном репозитории GIT отсутствует директория для объекта БД({DirPath})");
            }

            if (!File.Exists(GetRawFilePath()))
            {
                MessageBoxResult dialogResult = MessageBox.Show("Не найден файл " + GetRawFilePath() + ". Хотите создать его?", "Файл отсутствует в локальном репозитории", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (dialogResult == MessageBoxResult.Yes)
                    File.WriteAllText(GetRawFilePath().ToUpper(), "\r\n/");
                else
                    throw new Exception($"В локальном репозитории GIT отсутствует файл для сохранения объекта БД ({GetRawFilePath()})");
            }
        }
    }
}
