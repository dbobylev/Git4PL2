﻿using Git4PL2.Plugin.TeamCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    public interface ITeamCodingChecks
    {
        eTeamCodingChecksResult CheckBeforeCompile(out string ErrorMsg);

        eTeamCodingChecksResult CheckBeforeOpen(out string ErrorMsg);
    }
}
