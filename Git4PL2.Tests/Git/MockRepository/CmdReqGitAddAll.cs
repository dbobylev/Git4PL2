using Git4PL2.Git;
using Git4PL2.Git.CmdReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Tests.Git.MockRepository
{
    class CmdReqGitAddAll : CmdBuilderGIT
    {
        public CmdReqGitAddAll(string GitRepPath) : base(new CmdReadString(), GitRepPath)
        {
            AddArgumentGitRepPath();
            AddArgument("add .");
        }
    }
}
