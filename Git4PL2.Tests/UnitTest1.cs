using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Git4PL2.Tests
{
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            API a = new API();

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            string defPath = @"C:\Program Files (x86)\Microsoft Visual Studio\";
            string str = Directory.GetFiles(defPath, "dumpbin.exe", SearchOption.AllDirectories).Where(x=>x.Contains("\\x64\\dumpbin.exe")).First();

            string str2 = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "Git4PL2.dll", SearchOption.TopDirectoryOnly).First();


            Console.WriteLine(str);
            Console.WriteLine(str2);

            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.StandardOutputEncoding = new UTF8Encoding();
                p.StartInfo.FileName = str;
                p.StartInfo.Arguments = "/exports " + str2;
                p.Start();

                string standard_output;
                while ((standard_output = p.StandardOutput.ReadLine()) != null)
                {
                    Console.WriteLine(standard_output);
                }

            }
        }
    }
}
