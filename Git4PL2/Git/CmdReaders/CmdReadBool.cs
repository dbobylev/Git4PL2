using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdReaders
{
    class CmdReadBool : CmdReader<bool>
    {
        protected override void ReadOutputLine(string text)
        {
            Result = Result || !string.IsNullOrEmpty(text);
        }
    }
}
