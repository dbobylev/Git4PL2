using Git4PL2.Abstarct;
using Git4PL2.IDE.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.IDE.SQL
{
    /// <summary>
    /// Класс для выполнения запроса SQL на стороне PL/SQL Developer
    /// </summary>
    /// <typeparam name="T">Тив в котором мы сохраним результат select-а</typeparam>
    class SQLQueryExecute<T> :ISQLQueryExecute<T> where T:new() 
    {
        private ICallbackManager _CallbackManager;

        public string SelectQuery { get; set; }

        public SQLQueryExecute(ICallbackManager CallbackManager)
        {
            _CallbackManager = CallbackManager;
        }

        /// <summary>
        /// Запустить выполнение запроса SQL 
        /// </summary>
        /// <param name="ans">Результирующий ответ упакованный в тип Т</param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool RunSQLSelectQuery(out List<T> ans, out string ErrorMsg)
        {
            ans = new List<T>();

            // Выполняем запрос
            if (!GetResult(out SQLResult result, out string[] Headers, out ErrorMsg))
                return false;

            // Парсим ответ в лист ожидаемого типа T
            foreach (SQLRow row in result)
                ans.Add(row.GetObj<T>(Headers));

            return true;
        }

        private bool GetResult(out SQLResult result, out string[] Headers, out string ErrorMsg)
        {
            result = new SQLResult();
            Headers = null;

            if (!RunExecuteQuery(out ErrorMsg))
                return false;

            // Делегаты для работы с PL/SQL Developer
            SQL_FieldName SQL_FieldNameCallback = _CallbackManager.GetDelegate<SQL_FieldName>();
            SQL_Eof SQL_EofCallback = _CallbackManager.GetDelegate<SQL_Eof>();
            SQL_Field SQL_FieldCallback = _CallbackManager.GetDelegate<SQL_Field>();
            SQL_Next SQL_NextCallback = _CallbackManager.GetDelegate<SQL_Next>();

            int fieldCount = GetFieldCount();

            // Чтение заголовков
            Headers = new string[fieldCount];
            for (int i = 0; i < fieldCount; i++)
                Headers[i] = SQL_FieldNameCallback(i);

            Seri.Log.Here().Information("Начинаем вытягивать SQL_EofCallback");

            // Чтение данных
            while (!SQL_EofCallback())
            {
                object[] row = new object[fieldCount];
                for (int i = 0; i < fieldCount; i++)
                {
                    row[i] = SQL_FieldCallback(i);
                }
                result.AddRow(new SQLRow(row));
                SQL_NextCallback();
            }

            Seri.Log.Here().Information("SQL_EofCallback готов");

            return true;
        }

        private bool RunExecuteQuery(out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            Seri.Log.Here().Debug("Запрос выполнеия SQL на сервере. sql: {0}", SelectQuery);
            int SqlAns = _CallbackManager.GetDelegate<SQL_Execute>()?.Invoke(SelectQuery) ?? -1;
            Seri.Log.Here().Verbose($"SqlAns={SqlAns}");

            if (SqlAns == -1)
                throw new Exception("Не удалось вызвать callback - SQL_Execute");
            else if (SqlAns != 0)
            {
                string error = _CallbackManager.GetDelegate<SQL_ErrorMessage>()?.Invoke();
                Seri.Log.Here().Error("Ошибка при выполнении запроса: {0}", error);
                ErrorMsg = $"Error: ORA-{SqlAns}\r\n{error}";
                return false;
            }
            return true;
        }

        private int GetFieldCount()
        {
            int fieldCount = _CallbackManager.GetDelegate<SQL_FieldCount>()?.Invoke() ?? 0;
            Seri.Log.Here().Verbose("fieldCount={0}", fieldCount);
            return fieldCount;
        }
    }
}
