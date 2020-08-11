using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git
{
    abstract class CmdCore
    {
        private int CntRowsReaded;
        readonly string OperationName;
        protected string ProcessFileName;
        protected string ProcessArgs;

        // Класс который отвечает за интерпритацию результата консольной команды
        protected ICmdReader Reader;
        // Результат работы процесса сохранен в ридере
        public T GetResult<T>() => Reader.GetResult<T>();

        protected CmdCore(ICmdReader reader)
        {
            OperationName = this.GetType().Name;
            Reader = reader;
        }

        public void Run()
        {
            BeforeProcess();

            try
            {
                string ErrorMsg;
                using (Process p = GetNewProcces())
                {
                    p.Start();
                    CntRowsReaded = Reader.ReadProcess(p);
                    ErrorMsg = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                }
                /* Git вполне себе может выдать результат в StdError
                 * https://stackoverflow.com/questions/34820975/git-clone-redirect-stderr-to-stdout-but-keep-errors-being-written-to-stderr
                 */
                if (   !string.IsNullOrEmpty(ErrorMsg) 
                    && !ErrorMsg.StartsWith("Switched to a new branch")
                    && !ErrorMsg.StartsWith("Switched to branch"))
                    throw new Exception(ErrorMsg);
            }
            catch (Exception ex)
            {
                Seri.Log.Here().Error(ex, $"При выполнении процесса {OperationName} произошла ошибка: {ex.Message}");
                OnErrorOccurred();
                throw;
            }

            AfterProcess();
        }

        protected virtual void BeforeProcess()
        {
            Seri.Log.Here().Debug($"Запускаем процесс командной строки: {OperationName}");
        }
        protected virtual void AfterProcess()
        {
            Seri.Log.Here().Debug($"Процесс завершен. Прочитано: {CntRowsReaded}");
        }
        protected virtual void OnErrorOccurred() { }

        private Process GetNewProcces()
        {
            Seri.Log.Here().Verbose("Создаём новый процесс");
            Seri.Log.Here().Verbose($"ProcessName={ProcessFileName}");
            Seri.Log.Here().Verbose($"Args={ProcessArgs}");
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.StandardOutputEncoding = new UTF8Encoding();
            p.StartInfo.FileName = ProcessFileName;
            p.StartInfo.Arguments = ProcessArgs;
            return p;
        }
    }
}
