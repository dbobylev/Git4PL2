using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Git4PL2.Git;
using Git4PL2.Plugin.Abstract;
using NUnit.Framework.Interfaces;

namespace Git4PL2.Tests.Git.MockRepository
{
    class GitRep
    {
        private string GitRepPath;
        private List<string> files;

        public ReadOnlyCollection<string> Files
        {
            get => files.AsReadOnly();
        }

        public GitRep(string gitRepPath)
        {
            GitRepPath = gitRepPath;
            files = new List<string>();

            if (!Directory.Exists(GitRepPath))
                Directory.CreateDirectory(GitRepPath);
        }

        private void PerformCommand(CmdCore cmd)
        {
            cmd.Run();
            Console.WriteLine($"{cmd.GetType().Name} answer:\n{cmd.GetResult<string>()}");
        }

        /// <summary>
        /// Создать репозиторий
        /// </summary>
        public void CreateRepository() => PerformCommand(new CmdReqGitCreateRepository(GitRepPath));

        /// <summary>
        /// Зафиксировать все изменения для последующего коммита
        /// </summary>
        public void StageAll() => PerformCommand(new CmdReqGitAddAll(GitRepPath));

        /// <summary>
        /// Сделать коммит
        /// </summary>
        /// <param name="CommitMsg">Сообщения коммита</param>
        public void DoCommit(string CommitMsg) => PerformCommand(new CmdReqGitCommit(CommitMsg, GitRepPath));

        /// <summary>
        /// Показать историю коммитов (отобразятся в output)
        /// </summary>
        public void ShowLog() => PerformCommand(new CmdReqGitLog(GitRepPath));

        /// <summary>
        /// Получить список коммитов (SHA)
        /// </summary>
        /// <returns>список sha коммитов</returns>
        public List<string> GetCommits()
        {
            CmdCore cmd = new CmdReqGitLog(GitRepPath, new CmdReadShaFromLog());
            cmd.Run();
            return cmd.GetResult<List<string>>();
        }

        /// <summary>
        /// Удалить репозиторий
        /// </summary>
        public void DeleteRepository()
        {
            // Снимаем "только четние" с git файлов
            foreach (string file in Directory.EnumerateFiles(Path.Combine(GitRepPath, ".git"), "*.*", SearchOption.AllDirectories))
                File.SetAttributes(file, FileAttributes.Normal);
            Directory.Delete(GitRepPath, true);
        }

        /// <summary>
        /// Добавить файл в репозиторий
        /// </summary>
        public void AddFile(IDbObjectRepository RepObj = null)
        {
            string FilePath;

            if (RepObj == null)
                FilePath = Path.Combine(GitRepPath, RandomString.GetFileName());
            else
                FilePath = RepObj.GetRawFilePath();

            using (StreamWriter sw = File.CreateText(FilePath))
            {
                for (int i = 0; i < 40; i++)
                    sw.WriteLine(RandomString.GetTxtRow());
            }
            files.Add(FilePath);
        }

        public void CheckoutBranch(string BranchName, bool CreateBranch)
        {
            PerformCommand(new CmdReqGitCheckout(BranchName, CreateBranch, GitRepPath));
        }
    }
}
