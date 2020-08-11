using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git4PL2.Git.CmdReaders;

namespace Git4PL2.Git.CmdRequests
{
    class CmdReqGitModifiedFiles : CmdBuilderGIT
    {
        public CmdReqGitModifiedFiles(string branch1, string branch2) : base(new CmdReadListString())
        {
            AddArgumentGitRepPath();
            AddArgument("diff");
            AddArgument("--name-status");
            AddArgument(branch1);
            AddArgument(branch2);
        }

        protected override void AfterProcess()
        {
            base.AfterProcess();

            (Reader as CmdReadListString).EditReuslt((x) => x.Select(i => i.Split()[1]).OrderBy(y => y).ToList());
        }
    }
}
