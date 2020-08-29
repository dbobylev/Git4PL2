using Git4PL2.Plugin.Diff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    interface IIDEProvider
    {
        T GetDbObject<T>(bool silent = false) where T : IDbObject;

        void SetStatusMessage(string text);

        string GetDatabaseConnection();

        bool SetText(string Text);

        int GetCurrentLine();

        List<T> SQLQueryExecute<T>(string query) where T : new();

        string GetSelectedText();
    }
}
