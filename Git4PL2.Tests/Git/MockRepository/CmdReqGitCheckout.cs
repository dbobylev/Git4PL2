using Git4PL2.Git;
using Git4PL2.Git.CmdReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Tests.Git.MockRepository
{
    class CmdReqGitCheckout : CmdBuilderGIT
    {
        public CmdReqGitCheckout(string BranchName, bool CreateBranch, string gitRepPath) : base(new CmdReadString(), gitRepPath)
        {
            AddArgumentGitRepPath();
            AddArgument("checkout");
            if (CreateBranch)
                AddArgument("-b");
            AddArgument(BranchName);
        }
    }
}
