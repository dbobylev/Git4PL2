using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using Git4PL2.Git;
using Git4PL2.Tests.Git.MockRepository;

namespace Git4PL2.Tests.Git
{
    [TestFixture]
    class GitAPITest
    {
        /// <summary>
        /// Тестовый репозиторий
        /// </summary>
        private static GitRep repository;

        /// <summary>
        /// Путь до тестового репозитория
        /// </summary>
        private static string GitRepPath = Path.Combine(Environment.CurrentDirectory, "GitRepTest");

        [Test]
        [Description("Получаем объект с информацией о коммите по его ключу(sha)")]
        public static void GetCommitInfo_AllFieldsHasValue()
        {
            string TestCommitSHA = repository.GetCommits().First();

            GitAPI cmd = new GitAPI(GitRepPath);
            CommitInfo commit = cmd.GetCommitInfo(TestCommitSHA);

            Console.WriteLine($"Commit INFO >>\n{commit}\n<<");

            Assert.NotZero(commit.SHA.Length, "SHA не заполнен");
            Assert.AreEqual(TestCommitSHA, commit.SHA, "SHA коммита не верный");
            Assert.NotZero(commit.Author.Length, "Автор не заполнен");
            Assert.NotZero(commit.EMail.Length, "EMail не заполнен");
            Assert.IsTrue(commit.Date > DateTime.Now.AddMinutes(-5), "Проблема с датой");
        }

        [TestCase(2, 2, 1)]
        [TestCase(2, 4, 3)]
        [TestCase(2, -1, 1)]
        [Description("Получаем строки после операции git blame")]
        public static void GitBLame_ReturnListOfRows(int BegLine, int EndLine, int ExceptedRowCnt)
        {
            GitAPI cmd = new GitAPI(GitRepPath);
            var ListOrBlameRows = cmd.GitBlame<List<string>>(repository.Files.Last(), BegLine, EndLine);
            Console.WriteLine("Blame Row(s):");
            ListOrBlameRows.ForEach(x => Console.WriteLine($"[{x}]"));

            Assert.IsTrue(ListOrBlameRows.Count(x => string.IsNullOrEmpty(x)) == 0, "Blame вернул пустую строку!");
            Assert.AreEqual(ExceptedRowCnt, ListOrBlameRows.Count, "Кол-во строк после операции Blame не верное");
            Assert.Pass();
        }

        [Test]
        [Description("Получаем информацию по коммитам после операции git blame на определенном наборе строк")]
        public static void GetBlameCommitsInfo()
        {
            GitAPI cmd = new GitAPI(GitRepPath);
            var commits = cmd.GetBlameCommitsInfo(repository.Files.Last(), 2, 4);
            Console.WriteLine($"Commit INFO >>\n{commits.First()}\n<<");
            Assert.AreEqual(1, commits.Count);
        }

        [TestCase(false, 38, Description = "Запрашиваем полную информацию о коммите")]
        [TestCase(true, 7, Description = "Запрашиваем краткую информацию о коммите")]
        [Description("Получаем текст коммита по результатам операции git show")]
        public static void GitShow_ReturnTextOfCommit(bool ShortStat, int ExceptedRowCount)
        {
            string TestCommitSHA = repository.GetCommits().First();
            GitAPI cmd = new GitAPI(GitRepPath);

            var CommitAsString = cmd.GitShow<string>(TestCommitSHA, ShortStat);
            Assert.AreEqual(ExceptedRowCount, CommitAsString.Count(x => x == '\n'));

            var CommitAsListString = cmd.GitShow<List<string>>(TestCommitSHA, ShortStat);
            Assert.AreEqual(ExceptedRowCount, CommitAsListString.Count());
        }

        [Test]
        public static void GitGetCurrentBranch()
        {
            string BranchMaster = "master";
            string Branch1 = "feature/SomeImportantTask";
            string Branch2 = "X123_ddE";

            GitAPI GitAPI = new GitAPI(GitRepPath);
            Assert.AreEqual(BranchMaster, GitAPI.GetCurrentBranch());

            repository.CheckoutBranch(Branch1, true);
            Assert.AreEqual(Branch1, GitAPI.GetCurrentBranch());

            repository.CheckoutBranch(Branch2, true);
            Assert.AreEqual(Branch2, GitAPI.GetCurrentBranch());

            repository.CheckoutBranch(Branch1, false);
            Assert.AreEqual(Branch1, GitAPI.GetCurrentBranch());

            repository.CheckoutBranch(BranchMaster, false);
            Assert.AreEqual(BranchMaster, GitAPI.GetCurrentBranch());
        }

        [OneTimeSetUp]
        [Description("Создаём тестовый репозиторий для всех тестов в классе. " +
                     "Внимание! Внесение изменений в тестовый репозиторий должно сопровождаться обратной совместимостью " +
                     "во избежания поломки существующих тестов.")]
        public static void InitRepository()
        {
            Console.WriteLine($"Here we go! GitRepositoryPath: {GitRepPath}");

            repository = new GitRep(GitRepPath);
            repository.CreateRepository();

            repository.AddFile();
            repository.AddFile();
            repository.StageAll();
            repository.DoCommit("init commit");
        }

        [OneTimeTearDown]
        [Description("Удаляем тестовый репозиторий, по завершению всех тестов в классе")]
        public static void DropRepository()
        {
            repository.DeleteRepository();
        }
    }
}
