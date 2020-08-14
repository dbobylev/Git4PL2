using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdReaders
{
    class CmdReadDiffText : CmdReader<IDiffText>
    {
        protected override void ReadOutputLine(string OutputLine)
        {
            Result.AddLine(OutputLine);
        }

        protected override void OnReadDone()
        {
            Result.AddLine("@end");
        }
    }
}
