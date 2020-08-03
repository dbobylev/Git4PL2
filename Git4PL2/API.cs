using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2
{
    public class API
    {
        [DllExport("About", CallingConvention = CallingConvention.Cdecl)]
        public static string About()
        {
            return "About";
        }

        [DllExport("Configure", CallingConvention = CallingConvention.Cdecl)]
        public static void Configure()
        {
            throw new NotImplementedException("Не реализовано");
        }

        public static string CreateMenuItem(int index)
        {
            return string.Empty;
        }
    }
}
