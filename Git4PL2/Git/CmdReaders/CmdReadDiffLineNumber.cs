using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Git4PL2.Git.CmdReaders
{
    class CmdReadDiffLineNumber : CmdReader<int?>
    {
        private int NumValA;
        private int NumValB;
        private int NumCntA;
        private int NumCntB;

        private bool _CheckLineFromOutput = false;
        private bool _SearchDone = false; 

        private int _SearchedLineNumber;

        public CmdReadDiffLineNumber(int searchedLineNumber)
        {
            _SearchedLineNumber = searchedLineNumber;
        }

        protected override void ReadOutputLine(string line)
        {
            Seri.Log.Here().Information("GitDiff: " + line);

            if (_SearchDone || line.StartsWith("diff") || line.StartsWith("index"))
                return;
            if (line.StartsWith("@"))
            {
                int[] nums = SplitLineNumbers(line);
                NumValA = nums[0];
                NumCntA = nums[1];
                NumValB = nums[2];
                NumCntB = nums[3];

                if (_SearchedLineNumber < NumValB)
                {
                    Result = _SearchedLineNumber - (NumValB - NumValA);
                    _SearchDone = true;
                }
                else if (_SearchedLineNumber == NumValB + NumCntB)
                {
                    Result = NumValA + NumCntA;
                    _SearchDone = true;
                }
                else if (_SearchedLineNumber < NumValB + NumCntB)
                {
                    _CheckLineFromOutput = true;
                }
                return;
            }

            if (_CheckLineFromOutput)
            {
                Seri.Log.Verbose($"Beg line: {line} NumValA: {NumValA}, NumValB: {NumValB}");


                if (_SearchedLineNumber == NumValB && !line.StartsWith("-"))
                {
                    if (!line.StartsWith("+"))
                        Result = NumValA;
                    _SearchDone = true;
                } 
                else if (line.StartsWith("-"))
                {
                    NumValA++;
                }
                else if (line.StartsWith("+"))
                {
                    NumValB++;
                }
                else
                {
                    NumValA++;
                    NumValB++;
                };

                Seri.Log.Verbose($"End line: {line} NumValA: {NumValA}, NumValB: {NumValB}");
            }
        }

        protected override void OnReadDone()
        {
            if (RowsReadedCount == 0)
                Result = _SearchedLineNumber;
            else if (!_SearchDone)
            {
                Result = _SearchedLineNumber - ((NumValB + NumCntB) - (NumValA + NumCntA));
            }
            Seri.Log.Here().Debug("Result Repository line: " + Result ?? "null");
        }

        private int[] SplitLineNumbers(string s)
        {
            int[] ans = new int[4];
            MatchCollection mc = Regex.Matches(s, @"\d+");
            for (int i = 0; i < mc.Count; i++)
            {
                ans[i] = int.Parse(mc[i].Value);
                if (i == 3)
                    return ans;
            }
            return ans;
        }
    }
}
