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
            kernel = new StandardKernel(new NinjectPluginModule());
        }

        public static T Get<T>()
        {
            try
            {
                return kernel.Get<T>();
            }
            catch(Exception ex)
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
    }
}
