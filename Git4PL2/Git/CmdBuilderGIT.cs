using Git4PL2.Plugin.Abstract;
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
        private readonly ISettings _Settings;

        public CmdBuilderGIT(ICmdReader reader, string gitRepPath = null) : base(reader)
        {
            ProcessFileName = DefaultGitExePath;

            _Settings = NinjectCore.Get<ISettings>();

            GitRepPath = (gitRepPath == null) ? _Settings.GitRepositoryPath : gitRepPath;
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
            if (_Settings.DiffCRLF)
                AddArgument("--ignore-cr-at-eol");
        }

        protected void AddArgumentSpaceAtEol()
        {
            if (_Settings.DiffEndSpace)
                AddArgument("--ignore-space-at-eol");
        }

        protected void AddArgumentGitRepPath()
        {
            AddArgument($"-C \"{GitRepPath}\"");
        }
    }
}
