using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdReaders
{
    class CmdReadCommitInfoFromBlame : CmdReader<List<CommitInfo>>
    {
        private const string CommitInfoPatterns = @"^\^?([0-9a-f]+)\s\(<(.*)>\s(.*)\s\d+\)";

        private string email;
        private string sha;
        private DateTime date;
        private Regex regex = new Regex(CommitInfoPatterns);
        private HashSet<string> UniqueSha = new HashSet<string>();

        protected override void ReadOutputLine(string OutputLine)
        {
            try
            {
                MatchCollection matches = regex.Matches(OutputLine);
                sha = matches[0].Groups[1].Value;
                email = matches[0].Groups[2].Value;
                date = DateTime.Parse(matches[0].Groups[3].Value, null, DateTimeStyles.RoundtripKind);
            }
            catch (Exception ex)
            {
                Seri.Log.Here().Error($"Cnt={RowsReadedCount} OutputLine=[{OutputLine}] pattern=[{CommitInfoPatterns}]");
                throw ex;
            }
            if (UniqueSha.Add(sha))
                Result.Add(new CommitInfo(sha, null, email, date));
        }
    }
}
