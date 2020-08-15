//using Git4PL.Features.GitDiff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git4PL2.Git.Abstract;
using Git4PL2.Git.CmdReaders;
using Git4PL2.Git.CmdRequests;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Model;

namespace Git4PL2.Git
{
    public class GitAPI : IGitAPI
    {
        private string _GitRepositoryPath;

        public GitAPI(string gitRepPath)
        {
            _GitRepositoryPath = gitRepPath;
        }

        public GitAPI(ISettings settings)
        {
            _GitRepositoryPath = settings.GitRepositoryPath;
        }

        /// <summary>
        /// Выполнить консольную команду и вернуть результат
        /// </summary>
        /// <typeparam name="T">Тип ожидаемого результата</typeparam>
        /// <param name="process">Класс запроса CmdRequest</param>
        /// <returns></returns>
        private static T PerformProcess<T>(CmdCore process)
        {
            process.Run();
            return process.GetResult<T>();
        }

        /// <summary>
        /// Получить название текущей ветки в локальном резпозитории git
        /// </summary>
        /// <returns></returns>
        public string GetCurrentBranch()
        {
            return PerformProcess<string>(new CmdReqGitGetCurrentBranch());
        }

        public IDiffText GitDiff(IDbObjectText dbObject)
        {
            return PerformProcess<IDiffText>(new CmdReqGitDiff(dbObject));
        }

        /// <summary>
        /// Вернуть результат операции git blame для заданного файла, для заданных строк
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="beginLine"></param>
        /// <param name="endline"></param>
        /// <param name="ShowEmail">Вместо автора отображать email</param>
        /// <returns></returns>
        public T GitBlame<T>(string FileName, int beginLine, int endline = -1, bool ShowEmail = false, ICmdReader CustomReader = null)
        {
            return PerformProcess<T>(new CmdReqGitBlame(FileName, beginLine, endline, ShowEmail, GetReader(CustomReader, typeof(T))));
        }

        /// <summary>
        /// Вернуть список коммитов, который определился после операции git blame
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="BegLine"></param>
        /// <param name="EndLine"></param>
        /// <returns></returns>
        public List<CommitInfo> GetBlameCommitsInfo(string FileName, int BegLine, int EndLine)
        {
            return GitBlame<List<CommitInfo>>(FileName, BegLine, EndLine, true, new CmdReadCommitInfoFromBlame());
        }

        /// <summary>
        /// Получить список файлов которые отличаются между версиями двух указанных веток
        /// </summary>
        /// <param name="branch1"></param>
        /// <param name="branch2"></param>
        /// <returns></returns>
        public List<string> GetModifiedFiles(string branch1, string branch2)
        {
            return PerformProcess<List<string>>(new CmdReqGitModifiedFiles(branch1, branch2));
        }

        /// <summary>
        /// Проверяем существует ли данная ветка в репозитории
        /// </summary>
        /// <param name="BranchName"></param>
        /// <returns></returns>
        public bool VerifyBracch(string BranchName)
        {
            return PerformProcess<bool>(new CmdReqGitVerify(BranchName));
        }

        /// <summary>
        /// Получить информацию по коммиту
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sha"></param>
        /// <param name="shortstat"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public T GitShow<T>(string sha, bool shortstat, ICmdReader reader = null)
        {
            return PerformProcess<T>(new CmdReqGitShowCommit(sha, shortstat, GetReader(reader, typeof(T))));
        }

        /// <summary>
        /// Получить инфомацию по коммиту в виде класса CommitInfo
        /// </summary>
        /// <param name="sha"></param>
        /// <returns></returns>
        public CommitInfo GetCommitInfo(string sha)
        {
            return GitShow<CommitInfo>(sha, true, new CmdReadCommitInfo());
        }

        private ICmdReader GetReader(ICmdReader reader, Type T)
        {
            if (reader == null)
            {
                if (T == typeof(string))
                    reader = new CmdReadString();
                else if (T == typeof(List<string>))
                    reader = new CmdReadListString();
                else if (T == typeof(bool))
                    reader = new CmdReadBool();
                else
                    throw new Exception($"Отсутствует Reader для типа {T.Name}");
            }
            return reader;
        }
    }
}