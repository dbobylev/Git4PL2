using System;
using System.Collections.Generic;
using System.Linq;

namespace Git4PL2.PLSqlDev.IDECallBacks
{
    /// <summary>
    /// Класс содержит работу с обратными вызовами PL/SQL Developer
    /// Иницализация обратных вызовов происходит в CallbacksKeys
    /// </summary>
    public static class Callbacks2
    {
        /// <summary>
        /// Версия PL/SQL Developer
        /// </summary>
        /// <returns>Версия PL/SQL Developer</returns>
        public static int Version()
        {
            //int version = GetDelegate<SYS_Version>()?.Invoke() ?? -1;
            return 123;
        }
        /*
        /// <summary>
        /// Создать кнопку на панели PL/SQL Developer
        /// </summary>
        /// <param name="ID">ID Плагина</param>
        /// <param name="Index">Index кнопки</param>
        /// <param name="Name">Имя кнопки</param>
        /// <param name="BitmapFile"></param>
        /// <param name="BitmapHandle">Указатель на изображение</param>
        public static void CreateToolButton(int ID, int Index, string Name, string BitmapFile, IntPtr BitmapHandle)
        {
            GetDelegate<IDE_CreateToolButton>()?.Invoke(ID, Index, Name, BitmapFile, BitmapHandle);
        }

        /// <summary>
        /// Установить текст в открытом окне PL/SQL Developer
        /// </summary>
        /// <param name="Text">Текст</param>
        /// <returns></returns>
        public static bool SetText(string Text)
        {
            logger.Debug("Устанавливаем текст в окно PL/SQL Developer. text.length={0}", Text.Length);

            // Проверяем доступно ли для редактирования окно PL/SQL Developer
            bool IsPLSQLWindowReadonly = GetDelegate<IDE_GetReadOnly>()?.Invoke() ?? false;
            logger.Trace("Окно PL/SQL Developer {0}доступно для редактирвоания", IsPLSQLWindowReadonly ? "не " : "");

            if (IsPLSQLWindowReadonly)
            {
                DialogResult dialogResult = MessageBox.Show("В PL/SQL Developer объект БД открыт в режиме только для чтения! Сделать его редактируемым?", "View -> Edit", MessageBoxButtons.YesNo);
                if (dialogResult != DialogResult.Yes)
                    throw new Git4PLException("Окно с объектом БД не доступно для редактирования");

                // Снимаем с окна PL/SQL Developer свойство ReadOnly
                GetDelegate<IDE_SetReadOnly>()?.Invoke(false);
            }

            int CursorYPos = GetDelegate<IDE_GetCursorY>()?.Invoke() ?? 1;

            // Устанавливаем текст
            bool ans = GetDelegate<IDE_SetText>()?.Invoke(Text) ?? false;
            logger.Debug("Текст {0}установлен", ans ? "" : "не ");

            GoToLine(CursorYPos, 1);

            return ans;
        }

        /// <summary>
        /// Устанавливаем сообщение в трее PL/SQL Developer
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static bool SetStatusMessage(string Text)
        {
            Text = $"[{DateTime.Now.ToString("hh:mm:ss")}] {Text}";
            return GetDelegate<IDE_SetStatusMessage>()?.Invoke(Text) ?? false;
        }

        /// <summary>
        /// Получаем объект БД необходмый для рабоыт с CheckOut Checkin
        /// </summary>
        /// <returns>объект БД</returns>
        public static T GetDbObject<T>() where T : DbObject
        {
            int windowType = GetWindowType();
            if (windowType != 4 && windowType != 3)
            {
                throw new Git4PLException("Объект БД должен находиться в прграмном окне");
            }

            string ObjectType = string.Empty;
            string ObjectOwner = string.Empty;
            string ObjectName = string.Empty;
            string SubObject = string.Empty;

            logger.Debug("Запрос объекта БД");
            bool ans = GetDelegate<IDE_GetWindowObject>()?.Invoke(out ObjectType, out ObjectOwner, out ObjectName, out SubObject) ?? false;

            if (ans)
            {
                logger.Debug($"Объект получен ObjectType={ObjectType}, ObjectOwner={ObjectOwner},  ObjectName={ObjectName}, SubObject={SubObject}");

                // Получаем текст откртытого окна PL/SQL Developer
                string text = GetDelegate<IDE_GetText>()?.Invoke();

                // В некоторых версиях PL/SQL Developer (12.0.7.1837 32bit) есть баг! Вместо "PACKAGE BODY" возвращается "PACKAGE". (Версия Oracle - 18)
                // Если баги будут продолжаться то лучше отказаться от использования IDE_GetWindowObject и парсить название объекта прямо из текста
                if (ObjectType == "PACKAGE" || ObjectType == "TYPE")
                {
                    if (PLSQLCodeFormatter.HasBodyWord(text))
                    {
                        ObjectType += "BODY";
                        logger.Trace("Обноружено 'body' в название типа, изменяем тип на: {0}", ObjectType);
                    }
                }

                DbObject dbObj = new DbObject(ObjectOwner, ObjectName, ObjectType);
                if (dbObj is T ansdbObj)
                    return ansdbObj;

                DbObjectText dbObjText = new DbObjectText(dbObj, text);
                if (dbObjText is T ansdbObjText)
                    return ansdbObjText;
            }
            return default(T);
        }

        /// <summary>
        /// Поулчаем сервер к которому подключен пользовтель. (Сервер активного окна в PL/SQL Developer)
        /// </summary>
        /// <returns>Имя сервера</returns>
        public static string GetDatabaseConnection()
        {
            logger.Trace("Запрос соединения БД");
            string Database = string.Empty;
            int windowConnectionID = GetDelegate<IDE_GetWindowConnection>()?.Invoke() ?? -1;
            logger.Trace("windowConnectionID={0}", windowConnectionID);
            GetDelegate<IDE_GetConnectionInfoEx>()?.Invoke(windowConnectionID, out string Username, out string Password, out Database, out string ConnectAs);
            logger.Info("Cоединение БД: {0}", Database);
            return Database;
        }

        /// <summary>
        /// Получаем выделенный текст в окне PL/SQL Developer
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedText()
        {
            return GetDelegate<IDE_GetSelectedText>()?.Invoke();
        }

        public static bool SQLQueryExecute<T>(string query, out List<T> result, out string ErrorMsg) where T:new()
        {
            SQLQueryExecute<T> s = new SQLQueryExecute<T>(query);
            return s.RunSQLSelectQuery(out result, out ErrorMsg);
        }

        /// <summary>
        /// Проверяем установленно ли соединение
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectionAlive()
        {
            logger.Trace("Проверяем наличие соединения");
            IDE_Connected IDE_ConnectedCallback = GetDelegate<IDE_Connected>();
            if (IDE_ConnectedCallback == null)
            {
                logger.Trace("Делегат не установлен. Выходим.");
                return false;
            }
            bool ans = IDE_ConnectedCallback?.Invoke() ?? false;
            logger.Trace("Соединение {0}установлено", ans? "" : "НЕ ");
            return ans;
        }

        /// <summary>
        /// Получаем тип окна PL/SQL Developer
        /// </summary>
        /// <returns></returns>
        public static int GetWindowType()
        {
            /*  1 = SQL Window
                2 = Test Window
                3 = Procedure Window
                4 = Command Window
                5 = Plan Window
                6 = Report Window
                0 = None of the above*

            int ans = GetDelegate<IDE_GetWindowType>()?.Invoke() ?? 0;
            logger.Trace("Тип окна PL/SQL Developer - {0}", ans);
            return ans;
        }

        /// <summary>
        /// Получаем номер строки на которой стоит курсор
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentLine()
        {
            return GetDelegate<IDE_GetCursorY>()?.Invoke() ?? 1;
        }

        /// <summary>
        /// Перейти к строке
        /// </summary>
        /// <param name="LineNum"></param>
        /// <param name="posX"></param>
        public static void GoToLine(int LineNum, int BasePos = -1)
        {
            logger.Debug("GoToLine begin: LineNum={0}, BasePos={1}", LineNum, BasePos);
            /* При переходе к строке курсор занимает крайнюю к экрану строку!
             * Например: Если мы переходим к строке которая находится ниже нас, 
             * После перехода экран будет на нужной строке, но она будет в самом конце экрана а не по середине
             * сответственно, обратная ситуация при переходе наверх.
             * Пробуем это обойти двойным вызовом перехода с запасом +- 20 строк, (в зависимости в какую сторону идём вверх или вниз)
             * Что бы оказаться посередине экрана!
             *
            if (BasePos == -1)
                BasePos = GetDelegate<IDE_GetCursorY>()?.Invoke() ?? 1;
            
            int FakeLines = Math.Sign(LineNum - BasePos + 0.1d) * 20;
            GetDelegate<IDE_SetCursor>()?.Invoke(1, LineNum + FakeLines);
            
            GetDelegate<IDE_SetCursor>()?.Invoke(1, LineNum);
        }
        */
    }
}
