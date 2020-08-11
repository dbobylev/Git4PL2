using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Abstarct
{
    interface IPlsqlCodeFormatter
    {
        void UpdateBeginOfText(ref string SourceText, string Schema, string Name);

        void UpdateLastLines(ref string SourceText);

        void UpdateNewLines(ref string SourceText);

        bool IsBom();

        void RemoveSlash(ref string SourceText);

        bool HasBodyWord(string text);
    }
}
