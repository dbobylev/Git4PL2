using Git4PL2.Git;
using Git4PL2.Git.CmdReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Tests.Git.MockRepository
{
    class CmdReqGitCommit : CmdBuilderGIT
    {
        public CmdReqGitCommit(string message, string GitRepPath) :base(new CmdReadString(), GitRepPath)
        {
            AddArgumentGitRepPath();
            AddArgument("commit");
            AddArgument("-a");
            AddArgument($"-m \"{message}\"");
        }
    }
}
