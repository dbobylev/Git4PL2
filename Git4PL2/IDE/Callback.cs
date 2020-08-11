using Git4PL2.Abstarct;
using System;
using System.Runtime.InteropServices;

namespace Git4PL2.IDE
{
    /// <summary>
    /// Класс который хранит делегат для обратного вызова из PL/SQL Developer
    /// </summary>
    /// <typeparam name="T">Тип делегата</typeparam>
    class Callback<T> : ICallback where T : Delegate
    {
        public T CallBackDelegate;

        public Type delegateType
        {
            get { return typeof(T); }
        }

        public Callback()
        {

        }

        public P GetDelegate<P>()
        {
            if (CallBackDelegate is P AnswerDelegate)
                return AnswerDelegate;

            return default;
        }

        /// <summary>
        /// PL/SQL Developer устанавливает указатель для этого делагата
        /// </summary>
        /// <param name="function"></param>
        public void SetDelegate(IntPtr function)
        {
            CallBackDelegate = Marshal.GetDelegateForFunctionPointer<T>(function);
        }
    }
}
