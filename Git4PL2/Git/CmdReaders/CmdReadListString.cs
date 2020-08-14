﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdReaders
{
    class CmdReadListString : CmdReader<List<string>>
    {
        protected override void ReadOutputLine(string OutputLine)
        {
            Result.Add(OutputLine);
        }
    }
}
