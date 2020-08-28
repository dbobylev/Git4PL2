using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.TeamCoding
{
    public interface ITeamCodingProviderChecks
    {
        bool ChecksBeforeCheckIn(IEnumerable<ICheckOutObject> list, ICheckOutObject checkOutObject, out string ErrorMsg);

        bool ChecksBeforeCheckOut(IEnumerable<ICheckOutObject> list, ICheckOutObject checkOutObject, out string ErrorMsg);
    }
}
