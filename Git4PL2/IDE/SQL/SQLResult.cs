using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.IDE.SQL
{
    /// <summary>
    /// Результат выполнения SQL запроса как список строк с данными
    /// </summary>
    public class SQLResult : IEnumerable<SQLRow>
    {
        private List<SQLRow> rows;

        public SQLResult()
        {
            rows = new List<SQLRow>();
        }

        public void AddRow(SQLRow row)
        {
            rows.Add(row);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<SQLRow> GetEnumerator()
        {
            for (int i = 0; i < rows.Count; i++)
            {
                yield return rows[i];
            }
        }
    }
}
