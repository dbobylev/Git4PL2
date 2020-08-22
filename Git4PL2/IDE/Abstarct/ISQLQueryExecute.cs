using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.IDE.Abstarct
{
    public interface ISQLQueryExecute<T>
    {
        string SelectQuery { get; set; }

        bool RunSQLSelectQuery(out List<T> ans, out string ErrorMsg);
    }
}
