using Ninject;
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
            return kernel.Get<T>();
        }
    }
}
