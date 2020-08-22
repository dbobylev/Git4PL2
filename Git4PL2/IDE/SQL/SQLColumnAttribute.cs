using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.IDE.SQL
{
    [AttributeUsage(AttributeTargets.Property)]
    class SQLColumnAttribute : Attribute
    {
        public string ColumnName { get; set; }

        public SQLColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
