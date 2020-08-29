using Git4PL2.Abstarct;
using Git4PL2.IDE;
using Git4PL2.IDE.Abstarct;
using Git4PL2.IDE.SQL;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Git4PL2.Plugin.Commands
{
    class IDEProvider : IIDEProvider
    {
        private readonly ICallbackManager _CallbackManager;
        private readonly IPlsqlCodeFormatter _PlsqlCodeFormatter;

        public IDEProvider(ICallbackManager CallbackManager, IPlsqlCodeFormatter PlsqlCodeFormatter)
        {
            _CallbackManager = CallbackManager;
            _PlsqlCodeFormatter = PlsqlCodeFormatter;
        }

        public T GetDbObject<T>(bool silent = false) where T: IDbObject
        {
            Seri.Log.Here().Debug("Begin GetDbObject");

            int IDEWindowType = _CallbackManager.GetDelegate<IDE_GetWindowType>()?.Invoke() ?? 0;

            if (IDEWindowType != 4 && IDEWindowType != 3)
            {
                if (silent)
                    return default;
                else
                    throw new Exception("Объект БД должен находиться в прграмном окне");
            }

            string ObjectType = string.Empty;
            string ObjectOwner = string.Empty;
            string ObjectName = string.Empty;
            string SubObject = string.Empty;

            bool GetWindowObjectResult = _CallbackManager.GetDelegate<IDE_GetWindowObject>()?.Invoke(
                out ObjectType,
                out ObjectOwner,
                out ObjectName,
                out SubObject) ?? false;

            if (GetWindowObjectResult)
            {
                Seri.Log.Here().Debug($"Объект получен ObjectType={ObjectType}, ObjectOwner={ObjectOwner},  ObjectName={ObjectName}, SubObject={SubObject}");

                // Получаем текст откртытого окна PL/SQL Developer
                string text = _CallbackManager.GetDelegate<IDE_GetText>()?.Invoke();

                // В некоторых версиях PL/SQL Developer (12.0.7.1837 32bit) есть баг! Вместо "PACKAGE BODY" возвращается "PACKAGE". (Версия Oracle - 18)
                // Если баги будут продолжаться то лучше отказаться от использования IDE_GetWindowObject и парсить название объекта прямо из текста
                if (ObjectType == "PACKAGE" || ObjectType == "TYPE")
                {
                    if (_PlsqlCodeFormatter.HasBodyWord(text))
                    {
                        ObjectType += "BODY";
                        Seri.Log.Here().Verbose("Обноружено 'body' в название типа, изменяем тип на: {0}", ObjectType);
                    }
                }

                DbObject dbObject = new DbObject(ObjectOwner, ObjectName, ObjectType);
                if (dbObject is T ansDbObject)
                    return ansDbObject;

                DbObjectRepository dbObjectRepository = new DbObjectRepository(ObjectOwner, ObjectName, ObjectType);
                if (dbObjectRepository is T ansDbObjectRepository)
                    return ansDbObjectRepository;

                DbObjectText dbObjectText = new DbObjectText(dbObjectRepository, text);
                if (dbObjectText is T ansDbObjectText)
                    return ansDbObjectText;
            }

            return default;
        }

        public void SetStatusMessage(string Text)
        {
            Text = $"[{DateTime.Now:hh:mm:ss}] {Text}";
            _CallbackManager.GetDelegate<IDE_SetStatusMessage>()?.Invoke(Text);
        }

        public string GetDatabaseConnection()
        {
            Seri.Log.Here().Verbose("Запрос соединения БД");
            string Database = string.Empty;
            int windowConnectionID = _CallbackManager.GetDelegate<IDE_GetWindowConnection>()?.Invoke() ?? -1;
            Seri.Log.Here().Verbose("windowConnectionID={0}", windowConnectionID);
            _CallbackManager.GetDelegate<IDE_GetConnectionInfoEx>()?.Invoke(windowConnectionID, out string Username, out string Password, out Database, out string ConnectAs);
            Seri.Log.Here().Verbose("Cоединение БД: {0}", Database);
            return Database;
        }

        public bool SetText(string Text)
        {
            Seri.Log.Here().Debug("Устанавливаем текст в окно PL/SQL Developer. text.length={0}", Text.Length);

            // Проверяем доступно ли для редактирования окно PL/SQL Developer
            bool IsPLSQLWindowReadonly = _CallbackManager.GetDelegate<IDE_GetReadOnly>()?.Invoke() ?? false;
            Seri.Log.Here().Verbose("Окно PL/SQL Developer {0}доступно для редактирвоания", IsPLSQLWindowReadonly ? "не " : "");

            if (IsPLSQLWindowReadonly)
            {
                MessageBoxResult dialogResult = MessageBox.Show("В PL/SQL Developer объект БД открыт в режиме только для чтения! Сделать его редактируемым?", "View -> Edit", MessageBoxButton.YesNo);
                if (dialogResult != MessageBoxResult.Yes)
                    throw new Exception("Окно с объектом БД не доступно для редактирования");

                // Снимаем с окна PL/SQL Developer свойство ReadOnly
                _CallbackManager.GetDelegate<IDE_SetReadOnly>()?.Invoke(false);
            }

            int CursorYPos = _CallbackManager.GetDelegate<IDE_GetCursorY>()?.Invoke() ?? 1;

            // Устанавливаем текст
            bool ans = _CallbackManager.GetDelegate<IDE_SetText>()?.Invoke(Text) ?? false;
            Seri.Log.Here().Debug("Текст {0}установлен", ans ? "" : "не ");

            GoToLine(CursorYPos, 1);

            return ans;
        }

        private void GoToLine(int LineNum, int BasePos = -1)
        {
            Seri.Log.Here().Verbose("GoToLine begin: LineNum={0}, BasePos={1}", LineNum, BasePos);
            /* При переходе к строке курсор занимает крайнюю к экрану строку!
             * Например: Если мы переходим к строке которая находится ниже нас, 
             * После перехода экран будет на нужной строке, но она будет в самом конце экрана а не по середине
             * сответственно, обратная ситуация при переходе наверх.
             * Пробуем это обойти двойным вызовом перехода с запасом +- 20 строк, (в зависимости в какую сторону идём вверх или вниз)
             * Что бы оказаться посередине экрана!
             */
            if (BasePos == -1)
                BasePos = _CallbackManager.GetDelegate<IDE_GetCursorY>()?.Invoke() ?? 1;

            int FakeLines = Math.Sign(LineNum - BasePos + 0.1d) * 20;
            _CallbackManager.GetDelegate<IDE_SetCursor>()?.Invoke(1, LineNum + FakeLines);
            _CallbackManager.GetDelegate<IDE_SetCursor>()?.Invoke(1, LineNum);
        }

        public int GetCurrentLine()
        {
            return _CallbackManager.GetDelegate<IDE_GetCursorY>()?.Invoke() ?? 1;
        }

        public List<T> SQLQueryExecute<T>(string query) where T : new()
        {
            List<T> result;
            string ErrorMsg;

            ISQLQueryExecute<T> SqlQueryExecute = NinjectCore.Get<ISQLQueryExecute<T>>();
            SqlQueryExecute.SelectQuery = query;

            Seri.Log.Here().Information("Начинаем запрос " + query);

            if (!SqlQueryExecute.RunSQLSelectQuery(out result, out ErrorMsg))
            {
                // Возможно нужно обрабатывать по другому
                throw new Exception(ErrorMsg);
            }

            Seri.Log.Here().Information("Запрос выполнен");

            return result;
        }

        public string GetSelectedText()
        {
            return _CallbackManager.GetDelegate<IDE_GetSelectedText>()?.Invoke();
        }
    }
}
