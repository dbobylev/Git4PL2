using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git
{
    abstract class CmdBuilderGIT : CmdCore
    {
        private const string DefaultGitExePath = "git";
        private readonly string GitRepPath;

        public CmdBuilderGIT(ICmdReader reader, string gitRepPath = null) : base(reader)
        {
            ProcessFileName = DefaultGitExePath;
            GitRepPath = gitRepPath ?? Properties.Settings.Default.GitRepositoryPath;
        }

        protected void AddArgument(string text)
        {
            if (string.IsNullOrEmpty(ProcessArgs))
                ProcessArgs = text;
            else
                ProcessArgs += $" {text}";
        }

        protected void AddArgumentCrAtEol()
        {
            if (true)
                AddArgument("--ignore-cr-at-eol");
        }

        protected void AddArgumentSpaceAtEol()
        {
            if (true)
                AddArgument("--ignore-space-at-eol");
        }

        protected void AddArgumentGitRepPath()
        {
            AddArgument($"-C \"{GitRepPath}\"");
        }
    }
}
