using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.TeamCoding;
using Git4PL2.Plugin.TeamCoding.FileProvider;
using Ninject;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2
{
    public static class NinjectCore
    {
        private static IKernel kernel;
        static NinjectCore()
        {
            try
            {
                kernel = new StandardKernel(new NinjectPluginModule());
            }
            catch (Exception ex)
            {
                Seri.LogException(ex);
                throw;
            }
        }

        public static T Get<T>(params IParameter[] parameters)
        {
            try
            {
                return kernel.Get<T>(parameters);
            }
            catch (Exception ex)
            {
                Seri.LogException(ex);
                throw;
            }
        }

        public static IParameter GetParameter(string name, object obj)
        {
            return new ConstructorArgument(name, obj);
        }

        public static void SetTeamCodingProvider(eTeamCodingProviderType TeamCodingProviderType)
        {
            if (TeamCodingProviderType == eTeamCodingProviderType.ShareFileProvider)
                kernel.Rebind<ITeamCodingProvider>().To<TeamCodingFileProvider>();
        }
    }
}
