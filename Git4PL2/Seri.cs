using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Git4PL2
{
    public static class Seri
    {
        private static ILogger _log;
        public static ILogger Log
        {
            get
            {
                if (_log == null)
                    throw new ArgumentNullException("SeriLog not loaded");
                return _log;
            }
        }

        public static void LogException(Exception ex)
        {
            _log.Error("Message: " + ex.Message);
            _log.Error("Source: " + ex.Source);
            _log.Error("StackTrace: " + ex.StackTrace);
        }

        public static void SetModule(string name)
        {
            if (!string.IsNullOrEmpty(name))
                name = $"<{name}> ";
            _log = _log.ForContext("Module", name);
        }

        public static ILogger Here(this ILogger logger, [CallerMemberName] string memberName = "", [CallerFilePath] string file = "")
        {
            return logger.ForContext("MemberName", memberName).ForContext("file", Path.GetFileName(file));
        }

        static Seri()
        {
            // Лог хранится в папке %TEMP% для каждого запущенного PL/SQL Deveoper-a 
            // Лог живет только пока IDE работает и до последующего перезапуска

            string path = string.Empty;
            for (int i = 0; i < 100; i++)
            {
                path = Path.Combine(Path.GetTempPath(), $"Git4PL2_log_{i}.txt");
                try
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    break;
                } 
                catch (IOException)
                {
                    // Мы не можем получить доступ к логу, т.к. уже запущен PL/SQL Developer, создадим новый
                    continue;
                }
            }

            _log = new LoggerConfiguration().WriteTo.File(path, 
                outputTemplate: "{Timestamp:HH:mm:ss:fff} [{Level:u3}] [{Module}{file}:{MemberName}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Console()
                .MinimumLevel.Verbose().CreateLogger();
            SetModule(string.Empty);
        }
    }
}
