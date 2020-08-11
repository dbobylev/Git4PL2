using Git4PL2.Git;
using Git4PL2.Git.CmdReaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Git4PL2.Tests.Git.MockRepository
{
    class CmdReqGitLog :CmdBuilderGIT
    {
        public CmdReqGitLog(string GitRepPath, ICmdReader reader = null) :base (reader ?? new CmdReadString(), GitRepPath)
        {
            AddArgumentGitRepPath();
            AddArgument("log");
            AddArgument("--oneline");
            AddArgument("--no-abbrev-commit");
        }
    }
}
