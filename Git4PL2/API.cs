using Git4PL2.Abstarct;
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
        private static int _pluginID;
        private static IIDECallbacks _IDECallbacks = NinjectCore.Get<IIDECallbacks>();
        private static IMenu _Menu = NinjectCore.Get<IMenu>();

        [DllExport("IdentifyPlugIn", CallingConvention = CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int ID)
        {
            _pluginID = ID;
            return "Git4PL2";
        }

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

        [DllExport("CreateMenuItem", CallingConvention = CallingConvention.Cdecl)]
        public static string CreateMenuItem(int index)
        {
            return _Menu.CreateMenuItem(index); ;
        }

        [DllExport("RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            _IDECallbacks.SetDelegate(index, function);
        }

        [DllExport("OnActivate", CallingConvention = CallingConvention.Cdecl)]
        public static void OnActivate()
        {
            _Menu.CreateToolButtons(_pluginID);
        }

        [DllExport("OnMenuClick", CallingConvention = CallingConvention.Cdecl)]
        public static void OnMenuClick(int index)
        {
            _Menu.ClickOnMenu(index);
        }
    }
}
