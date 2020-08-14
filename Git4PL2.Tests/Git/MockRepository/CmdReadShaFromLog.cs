using Git4PL2.Git;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Git4PL2.Tests.Git.MockRepository
{
    class CmdReadShaFromLog : CmdReader<List<string>>
    {
        Regex regex = new Regex(@"^\w+");

        protected override void ReadOutputLine(string OutputLine)
        {
            Result.Add(regex.Matches(OutputLine)[0].Value);
        }
    }
}
