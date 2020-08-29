using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    public interface ITeamCodingProvider
    {
        IEnumerable<ICheckOutObject> GetCheckOutObjectsList();

        bool CheckOut(string Login, IDbObject dbObject, out string ErrorMsg);

        bool CheckIn(string Login, IDbObject dbObject, out string ErrorMsg);

        bool RestrickCompileWithoutCheckOut { get; set; }

        string GetUserOwner(IDbObject dbObject, string server);
    }
}
