using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    interface IWarnings
    { 
        bool IsBranchUnexsepted(string BranchName, bool SilentMode = false);

        bool IsServerUnexsepted(string ServerName, bool SilentMode = false);
    }
}
