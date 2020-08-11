using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git4PL2.Git.CmdReaders;

namespace Git4PL2.Git.CmdRequests
{
    class CmdReqGitShowCommit : CmdBuilderGIT
    {
        public CmdReqGitShowCommit(string sha, bool shortstat, ICmdReader reader) : base(reader)
        {
            AddArgumentGitRepPath();
            AddArgument("show");
            AddArgument("--date=iso");
            if (shortstat)
                AddArgument("--shortstat");
            AddArgument(sha);
        }
    }
}