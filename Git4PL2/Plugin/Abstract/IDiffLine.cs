using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git4PL2.Plugin.Model;

namespace Git4PL2.Plugin.Abstract
{
    public interface IDiffLine
    {
        string ToStringWithLineIndent(int indent);

        eDiffLineType Type { get; }
    }
}
