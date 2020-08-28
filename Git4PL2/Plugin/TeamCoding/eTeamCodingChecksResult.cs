using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.TeamCoding
{
    [Flags]
    public enum eTeamCodingChecksResult
    {
        None            = 0,
        Allow           = 1 << 0,
        Restrict        = 1 << 1,
        ProviderNotSet  = 1 << 2
    }
}
