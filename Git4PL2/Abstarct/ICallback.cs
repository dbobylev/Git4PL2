using System;

namespace Git4PL2.Abstarct
{
    interface ICallback
    {
        /// <summary>
        /// Тип делагата
        /// </summary>
        Type delegateType { get; }

        /// <summary>
        /// Получить делегат, для работы с ним
        /// </summary>
        /// <typeparam name="T">Тип делагата</typeparam>
        /// <returns></returns>
        T GetDelegate<T>();

        /// <summary>
        /// Устонавливает функцию, которую будет вызывать делегат
        /// </summary>
        /// <param name="function">Указаель на функцию, передается из PL/SQL Developer</param>
        void SetDelegate(IntPtr function);
    }
}
