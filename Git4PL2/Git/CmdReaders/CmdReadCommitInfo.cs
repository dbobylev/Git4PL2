using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdReaders
{
    class CmdReadCommitInfo : CmdReader<CommitInfo>
    {
        private readonly string[] CommitInfoPatterns =
            new string[3]{ @"^commit\s*([0-9a-f]+)$",
                           @"^Author:\s*(.*)\s+\<(.*)\>$",
                           @"^Date:\s*(.*)$"
                         };

        private string author;
        private string email;
        private string sha;
        private DateTime date;

        protected override void ReadOutputLine(string text)
        {
            if (RowsReadedCount < 3)
            {
                try
                {
                    Regex reg = new Regex(CommitInfoPatterns[RowsReadedCount]);
                    MatchCollection matches = reg.Matches(text);

                    switch (RowsReadedCount)
                    {
                        case 0:
                            sha = matches[0].Groups[1].Value;
                            break;
                        case 1:
                            author = matches[0].Groups[1].Value;
                            email = matches[0].Groups[2].Value;
                            break;
                        case 2:
                            date = DateTime.Parse(matches[0].Groups[1].Value, null, DateTimeStyles.RoundtripKind);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Seri.Log.Here().Error($"Cnt={RowsReadedCount} OutputLine=[{text}] pattern=[{CommitInfoPatterns[RowsReadedCount]}]");
                    throw ex;
                }
            }
        }

        protected override void OnReadDone()
        {
            Result = new CommitInfo(sha, author, email, date);
        }
    }
}
