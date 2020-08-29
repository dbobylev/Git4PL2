using Git4PL2.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Git4PL2.IDE
{
    public class CallbackManager : ICallbackManager
    {
        /// <summary>
        /// Словарь всех возвожных обратных вызовов, которые будут храниться в плагине
        /// </summary>
        private readonly Dictionary<int, ICallback> callbacks;

        /// <summary>
        /// Выбрать необходимый делегат из словаря callbacks
        /// </summary>
        /// <typeparam name="T">Тип делегата</typeparam>
        /// <returns>Делегат T</returns>
        public T GetDelegate<T>()
        {
            return callbacks.Select(x => x.Value).FirstOrDefault(x => x.delegateType == typeof(T)).GetDelegate<T>();
        }

        /// <summary>
        /// Установка делегата для конкретного обратного вызова. Делегаты устанавливает сам PL/SQL Developer при инициализации плагина.
        /// </summary>
        /// <param name="callbacktype">Тип обратного вызова</param>
        /// <param name="function">Указатель на функцию</param>
        public void SetDelegate(int index, IntPtr function)
        {
            if (!callbacks.ContainsKey(index))
                return;
            callbacks[index].SetDelegate(function);
        }

        public CallbackManager()
        {
            // Инициализация всех обратных вызовов доступных в плагине
            callbacks = new Dictionary<int, ICallback>()
            {
                {1,   new Callback<SYS_Version>() },
                {11,  new Callback<IDE_Connected>() },
                {14,  new Callback<IDE_GetWindowType>() },
                {25,  new Callback<IDE_SetReadOnly>() },
                {26,  new Callback<IDE_GetReadOnly>() },
                {30,  new Callback<IDE_GetText>() },
                {31,  new Callback<IDE_GetSelectedText>() },
                {34,  new Callback<IDE_SetText>() },
                {35,  new Callback<IDE_SetStatusMessage>() },
                {40,  new Callback<SQL_Execute>() },
                {41,  new Callback<SQL_FieldCount>() },
                {42,  new Callback<SQL_Eof>() },
                {43,  new Callback<SQL_Next>() },
                {44,  new Callback<SQL_Field>() },
                {45,  new Callback<SQL_FieldName>() },
                {46,  new Callback<SQL_FieldIndex>() },
                {48,  new Callback<SQL_ErrorMessage>() },
                {52,  new Callback<SQL_CheckConnection>() },
                {64,  new Callback<IDE_RefreshMenus>() },
                {110, new Callback<IDE_GetWindowObject>() },
                {141, new Callback<IDE_GetCursorX>() },
                {142, new Callback<IDE_GetCursorY>() },
                {143, new Callback<IDE_SetCursor>() },
                {150, new Callback<IDE_CreateToolButton>() },
                {240, new Callback<IDE_GetConnectionInfoEx>() },
                {245, new Callback<IDE_GetWindowConnection>() }
            };
        }
    }
}