using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git4PL2.Git.CmdReaders;

namespace Git4PL2.Git.CmdRequests
{
    class CmdReqGitBlame : CmdBuilderGIT
    {
        public CmdReqGitBlame(string fileName, int beginLine, int endline, bool ShowEmail, ICmdReader reader) : base(reader)
        {
            AddArgumentGitRepPath();
            AddArgument("blame");
            AddArgument("-w");
            AddArgument("-l");
            AddArgument($" -L {beginLine},{endline}");
            if (ShowEmail)
                AddArgument("--show-email");
            AddArgument($"{fileName}");
        }
    }
}
