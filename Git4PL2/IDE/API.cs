using Git4PL2.Abstarct;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.TeamCoding;
using RGiesecke.DllExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Git4PL2.IDE
{
    public class API
    {
        private static int _pluginID;
        private readonly static ICallbackManager _CallbackManager;
        private readonly static ITeamCodingChecks _TeamCodingChecks;
        private readonly static IMenu _Menu;

        static API()
        {
            try
            {
                _CallbackManager = NinjectCore.Get<ICallbackManager>();
                _Menu = NinjectCore.Get<IMenu>();
                _TeamCodingChecks = NinjectCore.Get<ITeamCodingChecks>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);
                Seri.Log.Here().Error(ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);
                throw;
            }
        }

        [DllExport("IdentifyPlugIn", CallingConvention = CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int ID)
        {
            _pluginID = ID;
            return "Git4PL2";
        }

        [DllExport("About", CallingConvention = CallingConvention.Cdecl)]
        public static string About()
        {
            return "About ^_^";
        }

        [DllExport("Configure", CallingConvention = CallingConvention.Cdecl)]
        public static void Configure()
        {
            throw new NotImplementedException("Не реализовано");
        }

        [DllExport("CreateMenuItem", CallingConvention = CallingConvention.Cdecl)]
        public static string CreateMenuItem(int index)
        {
            return _Menu.CreateMenuItem(index);
        }

        [DllExport("RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            _CallbackManager.SetDelegate(index, function);
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

        [DllExport("BeforeExecuteWindow", CallingConvention = CallingConvention.Cdecl)]
        public static bool BeforeExecuteWindow(int WindowType)
        {
            if (WindowType == 3 || WindowType == 4)
            {
                var result = _TeamCodingChecks.CheckBeforeCompile(out string ErrorMsg);
                Seri.Log.Here().Debug($"BeforeExecuteWindow ChackResult: {result}");

                if (result.HasFlag(eTeamCodingChecksResult.Restrict))
                {
                    MessageBox.Show(ErrorMsg, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else if (result.HasFlag(eTeamCodingChecksResult.Allow))
                {
                    return true;
                }
            }

            Seri.Log.Here().Warning("Здесь не должны оказаться!");
            return true;
        }

        [DllExport("OnWindowCreated", CallingConvention = CallingConvention.Cdecl)]
        public static void OnWindowCreated(int WindowType)
        {
            if (WindowType == 3 || WindowType == 4)
            {
                var result = _TeamCodingChecks.CheckBeforeOpen(out string ErrorMsg);
                Seri.Log.Here().Debug($"OnWindowCreated ChackResult: {result}");

                if (result.HasFlag(eTeamCodingChecksResult.ProviderNotSet))
                {
                    _CallbackManager.GetDelegate<IDE_SetStatusMessage>()?.Invoke($"Ошибка TeamCoding: {ErrorMsg}");
                }
                else if (result.HasFlag(eTeamCodingChecksResult.Restrict))
                {
                    _CallbackManager.GetDelegate<IDE_SetStatusMessage>()?.Invoke(ErrorMsg);
                }
            }
        }
    }
}
