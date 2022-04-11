using Git4PL2.Abstarct;
using Git4PL2.Plugin.Abstract;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Diff
{
    class PlsqlCodeFormatter: IPlsqlCodeFormatter
    {
        private readonly ISettings _Settings;

        private string GitText { get; set; }
        private string GitFilePath { get; set; }

        public PlsqlCodeFormatter(ISettings Settings)
        {
            _Settings = Settings;
        }

        public PlsqlCodeFormatter(string remoteFilePath, ISettings Settings)
        {
            Seri.Log.Here().Verbose("Загружаем текст из репозитория GIT");
            _Settings = Settings;
            GitText = File.ReadAllText(remoteFilePath);
            GitFilePath = remoteFilePath;
            Seri.Log.Here().Verbose("Текст загружен text.length={0}", GitText.Length);
        }

        public void UpdateBeginOfText(ref string SourceText, string Schema, string Name)
        {
            // cor - create or replace block
            bool DiffChangeCor = _Settings.DiffChangeCor;
            bool DiffChangeName = _Settings.DiffChangeName;
            bool DiffAddSchema = _Settings.DiffAddSchema;

            if (!(DiffChangeCor || DiffChangeName || DiffAddSchema))
                return;

            Seri.Log.Here().Verbose("Начинаем UpdateBeginOfText");
            Seri.Log.Here().Verbose("ChangeCor={0}", DiffChangeCor.ToString());
            Seri.Log.Here().Verbose("ChangeName={0}", DiffChangeName.ToString());
            Seri.Log.Here().Verbose("DiffAddSchema={0}", DiffAddSchema.ToString());

            string pattern = @"^(?<cor>create or replace[\n\r\s]+\w+[\n\r\s]+(body[\n\r\s]+)?)(?<name>[""]?(" + Schema + @")?[""]?\.?[""]?" + Name + @"[""]?)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            Match GitMatch = regex.Match(GitText, 0, Math.Min(200, GitText.Length));
            if (!(GitMatch.Groups["cor"].Success && GitMatch.Groups["name"].Success))
            {
                Seri.Log.Here().Warning("Не удалось найти совпадение регулярного выраженя в Git файле. pattern={0}", pattern);
                Seri.Log.Here().Verbose("Конец UpdateBeginOfText");
                return;
            }

            string GitCor = GitMatch.Groups["cor"].Value;
            Seri.Log.Here().Verbose("GitCor={0}", GitCor);
            string GitName = GitMatch.Groups["name"].Value;
            Seri.Log.Here().Verbose("GitName={0}", GitName);

            Match dbMatch = regex.Match(SourceText, 0, Math.Min(200, SourceText.Length));
            if (!(dbMatch.Groups["cor"].Success && dbMatch.Groups["name"].Success))
            {
                Seri.Log.Here().Warning("Не удалось найти совпадение регулярного выраженя в тексте объекта БД. pattern={0}", pattern);
                Seri.Log.Here().Verbose("Конец UpdateBeginOfText");
                return;
            }

            Group dbNameGroup = dbMatch.Groups["name"];
            string dbCor = dbMatch.Groups["cor"].Value;
            Seri.Log.Here().Verbose("dbCor={0}", dbCor);
            string dbName = dbNameGroup.Value;
            Seri.Log.Here().Verbose("dbName={0}", dbName);

            string textAfterName = SourceText.Substring(dbNameGroup.Index + dbNameGroup.Length);

            string FinalName = DiffChangeName ? GitName : dbName;
            if (DiffAddSchema && !FinalName.Contains('.'))
            {
                Seri.Log.Here().Verbose("Не найден префикс схемы у объекта БД");
                FinalName = $"{Schema}.{FinalName}";
                Seri.Log.Here().Verbose("Схема добавлена, FinalName={0}", FinalName);
            }

            Seri.Log.Here().Verbose("Обновляем тект объекта БД");
            SourceText = $"{(DiffChangeCor ? GitCor : dbCor)}{FinalName}{textAfterName}";

            Seri.Log.Here().Verbose("Конец UpdateBeginOfText");
        }

        /// <summary>
        /// Работа со слешом при сохранении объекта БД в репозитории.
        /// Логика:
        /// Ищем окончание состоящие из пробелов/переносов/слеша, в объекте БД и в репозитории
        /// Подставляем окончания из репозитория вместо окончания в объекте БД
        /// Если включена настройка, еще добавляем слеш
        /// </summary>
        /// <param name="SourceText"></param>
        public void UpdateLastLines(ref string SourceText)
        {
            if (_Settings.DiffSlashSettings == eEndSlashSettings.OptionDisabled)
                return;

            Seri.Log.Here().Verbose("Начинаем UpdateLastLines");
            // Патерн ищет окончание состоящие из переносов, пробелов или слеша.
            string pattern = @"[^\*](?<end>[\r\n\s/]+)$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            // Ищем совпадение в тексте в репозитоии
            Match GitMatch = regex.Match(GitText, GitText.Length - Math.Min(100, GitText.Length));

            string GitEnd = string.Empty;
            if (!GitMatch.Groups["end"].Success)
            {
                // Если патерн не нашли, то при включенной опции слеша, добавляем его в ручную, либо выходим
                Seri.Log.Here().Warning("Не удалось найти совпадение регулярного выраженя в Git файле. pattern={0}", pattern);
                if (_Settings.DiffSlashSettings == eEndSlashSettings.DontTouchSlash)
                {
                    Seri.Log.Here().Verbose("Конец UpdateLastLines");
                    return;
                }
                else if (_Settings.DiffSlashSettings == eEndSlashSettings.AlwaysAddSlash)
                {
                    Seri.Log.Here().Verbose("Добавляем слеш в ручную");
                    GitEnd = "\r\n/\r\n";
                }
            }
            else
            {
                // Паттерн нашли
                GitEnd = GitMatch.Groups["end"].Value;
                // Проверяем есть ли слеш в окончании GitEnd (Там могут быть только переносы/пробелы)
                if (!GitEnd.Contains("/") && _Settings.DiffSlashSettings == eEndSlashSettings.AlwaysAddSlash)
                {
                    Seri.Log.Here().Verbose("Добавляем слеш в ручную");
                    GitEnd = "\r\n/\r\n";
                }
            }

            Seri.Log.Here().Verbose("GitEnd(bytes)={0}", string.Join(",", GitEnd.Select(x => (int)x)));

            // Ищем такое-же окончание в тексте объекта БД из PL/SQL Developer,
            Match dbMatch = regex.Match(SourceText, SourceText.Length - Math.Min(100, SourceText.Length));

            string TextBeforeEnd = string.Empty;
            if (!dbMatch.Groups["end"].Success)
            {
                Seri.Log.Here().Warning("Не удалось найти совпадение регулярного выраженя в тексте объекта БД. pattern={0}", pattern);
                if (_Settings.DiffSlashSettings == eEndSlashSettings.DontTouchSlash)
                {
                    Seri.Log.Here().Verbose("Конец UpdateLastLines");
                    return;
                }
                // Если мы не нашли никакого окончания, то мы должны попробовать добавть слеш(если его еще нет)
                else if (_Settings.DiffSlashSettings == eEndSlashSettings.AlwaysAddSlash)
                {
                    // Мы не нашли совпадение поэтому TextBeforeEnd будет являеться всем текстом объекта
                    TextBeforeEnd = SourceText;
                }
            }
            else
            {
                string dbEnd = dbMatch.Groups["end"].Value;
                Seri.Log.Here().Verbose("dbEnd(bytes)={0}", string.Join(",", dbEnd.Select(x => (int)x)));
                TextBeforeEnd = SourceText.Substring(0, dbMatch.Groups["end"].Index);
            }
            Seri.Log.Here().Verbose("Обновляем тект объекта БД");

            // Добавляем концовку
            SourceText = $"{TextBeforeEnd}{GitEnd}";
            Seri.Log.Here().Verbose("Конец UpdateLastLines");
        }

        public void UpdateNewLines(ref string SourceText)
        {
            if (!_Settings.DiffCRLF)
                return;

            Seri.Log.Here().Verbose("Начало UpdateNewLines");
            string patternR = @"^[^\n]+[^\r][\n]";
            string patternRN = @"^[^\n]+[\r][\n]";
            Regex regexR = new Regex(patternR);
            Regex regexRN = new Regex(patternRN);

            if (regexRN.IsMatch(GitText))
            {
                Seri.Log.Here().Verbose("В файле Git используется перенос строк CR-LF");
            }
            else if (regexR.IsMatch(GitText))
            {
                Seri.Log.Here().Warning("В файле Git используется перенос LF");
                Seri.Log.Here().Verbose("Заменяем все переносы строк текста с CR-LF на LF");
                SourceText = SourceText.Replace("\r\n", "\n");
            }
            else
            {
                Seri.Log.Here().Warning("Не удалось распознать формат переноса строк в файле Git");
            }
            Seri.Log.Here().Verbose("Конец UpdateNewLines");
        }

        public bool IsBom()
        {
            var bom = new byte[4];
            using (var file = new FileStream(GitFilePath, FileMode.Open, FileAccess.Read))
                file.Read(bom, 0, 4);
            return bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf;
        }

        /// <summary>
        /// Удаляем все символы переноса, пробела и слеша включительно, с конца файла, пока не встретим слеш
        /// </summary>
        /// <param name="SourceText"></param>
        public void RemoveSlash(ref string SourceText)
        {
            if (_Settings.DiffSlashSettings == eEndSlashSettings.OptionDisabled)
                return;

            Seri.Log.Here().Verbose("Начинаем RemoveSlash");
            int cnt = 0;
            for (int i = SourceText.Length - 1; i > 0; i--)
            {
                char ch = SourceText[i];
                if (ch == '\r' || ch == '\n' || ch == ' ')
                {
                    cnt++;
                    continue;
                }
                else if (ch == '/')
                {
                    if (SourceText[i - 1] == '*')
                        break;
                    cnt++;
                    Seri.Log.Here().Verbose("Символ '/' найден. Будет удалено n={0} символов с конца файла", cnt);
                    SourceText = SourceText.Remove(SourceText.Length - cnt);
                    break;
                }
                break;
            }
            Seri.Log.Here().Verbose("Конец RemoveSlash");
        }

        public bool HasBodyWord(string text)
        {
            string pattern = @"create or replace[\n\r\s]+\w+[\n\r\s]+body";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Seri.Log.Here().Verbose("Проверка на наличие body в название типа, паттерн: {0}", pattern);
            bool hasBody = regex.IsMatch(text);
            Seri.Log.Here().Verbose("HasBodyWord={0}", hasBody);
            return hasBody;
        }
    }
}
