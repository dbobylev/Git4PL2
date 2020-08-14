using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdReaders
{
    class CmdReadStringBuilder : CmdReader<StringBuilder>
    {
        protected override void ReadOutputLine(string OutputLine)
        {
            Result.AppendLine(OutputLine);
        }
    }
}
