using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdReaders
{
    class CmdReadString : CMDReader<string>
    {
        protected override void ReadOutputLine(string OutputLine)
        {
            Result += OutputLine;
            Result += "\n";
        }

        protected override void OnReadDone()
        {
            if (!string.IsNullOrEmpty(Result))
                Result.TrimEnd('\n');
        }
    }
}
