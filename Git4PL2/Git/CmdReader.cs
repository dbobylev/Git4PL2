using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git
{
    abstract class CMDReader<T> : ICmdReader
    {
        protected T Result;
        protected int RowsReadedCount = 0;

        public CMDReader()
        {
            try
            {
                if (typeof(T).GetConstructor(Type.EmptyTypes) != null)
                    Result = Activator.CreateInstance<T>();
                else if (typeof(T) != typeof(string))
                    Seri.Log.Here().Warning($"Для типа {typeof(T).Name} отсутствует конструктор по умолчанию!");
            }
            catch (MissingMethodException ex)
            {
                Seri.Log.Here().Error($"Выбран не совместимый тип [{typeof(T).Name}] для ридера [{this.GetType().Name}]");
                throw ex;
            }
        }

        public int ReadProcess(Process p)
        {
            string standard_output;
            while ((standard_output = p.StandardOutput.ReadLine()) != null)
            {
                ReadOutputLine(standard_output);
                RowsReadedCount++;
            }
            OnReadDone();
            return RowsReadedCount;
        }

        protected abstract void ReadOutputLine(string OutputLine);
        protected virtual void OnReadDone() { }

        public void EditReuslt(Func<T, T> func)
        {
            Result = func(Result);
        }

        public P GetResult<P>()
        {
            if (Result is P ans)
                return ans;

            Seri.Log.Here().Warning($"Запрошен неправильный тип результата! ожидалось: [{typeof(T).Name}] запрошено: [{typeof(P).Name}]");
            return default;
        }
    }
}
