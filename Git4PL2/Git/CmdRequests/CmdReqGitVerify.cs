using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git4PL2.Git.CmdReaders;

namespace Git4PL2.Git.CmdRequests
{
    class CmdReqGitVerify: CmdBuilderGIT
    {
        public CmdReqGitVerify(string BranchName) : base(new CmdReadBool())
        {
            AddArgumentGitRepPath();
            AddArgument("rev-parse");
            AddArgument("--verify");
            AddArgument("--quiet");
            AddArgument(BranchName);
        }
    }
}
