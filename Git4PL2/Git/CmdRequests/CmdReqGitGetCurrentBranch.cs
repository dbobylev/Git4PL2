using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git4PL2.Git.CmdReaders;

namespace Git4PL2.Git.CmdRequests
{
    class CmdReqGitGetCurrentBranch : CmdBuilderGIT
    {
        public CmdReqGitGetCurrentBranch() : base(new CmdReadString())
        {
            AddArgumentGitRepPath();
            AddArgument("rev-parse");
            AddArgument("--abbrev-ref HEAD");
        }

        protected override void AfterProcess()
        {
            base.AfterProcess();

            (Reader as CmdReadString).EditReuslt((x) => x.Replace("\n", string.Empty));
        }
    }
}
