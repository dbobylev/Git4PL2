using Git4PL2.Plugin.Abstract;
using System.Linq;
using System.Windows.Media;

namespace Git4PL2.Plugin.Model
{
    /// <summary>
    /// Строка которая будет отображаться в окне GitDiff
    /// </summary>
    public class DiffLine: IDiffLine
    {
        public int? LineNumA { get; private set; }
        public int? LineNumB { get; private set; }
        public eDiffLineType Type { get; private set; }
        public string Line { get; private set; }

        public DiffLine(string pLine, int? pLineNumA = null, int? pLineNumB = null)
        {
            Line = pLine + "\r\n";
            switch (pLine.DefaultIfEmpty(' ').FirstOrDefault())
            {
                case '+': Type = eDiffLineType.Plus; break;
                case '-': Type = eDiffLineType.Minus; break;
                case '@': Type = eDiffLineType.Mail; break;
                default: Type = eDiffLineType.Default; break;
            }
            LineNumA = pLineNumA;
            LineNumB = pLineNumB;
        }

        /// <summary>
        /// Получить строку для отображения
        /// </summary>
        /// <param name="indent">ширина отступа</param>
        /// <returns></returns>
        public string ToStringWithLineIndent(int indent)
        {
            return string.Format("{0,"+indent+ "} {1," + indent + "} | {2}", LineNumA, LineNumB, Line);
        }
    }
}
