using Git4PL2.Git.CmdReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdRequests
{
    class CmdReqGitGetLastCommit : CmdBuilderGIT
    {
        public CmdReqGitGetLastCommit() : base(new CmdReadString())
        {
            AddArgumentGitRepPath();
            AddArgument("log");
            AddArgument("-n1");
            AddArgument("--format=\"%H\"");
        }

        protected override void AfterProcess()
        {
            base.AfterProcess();

            (Reader as CmdReadString).EditReuslt((x) => x.Replace("\n", string.Empty));
        }
    }
}
