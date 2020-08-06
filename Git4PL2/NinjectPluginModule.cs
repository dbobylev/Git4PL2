using Git4PL2.Abstarct;
using Git4PL2.PLSqlDev;
using Git4PL2.PLSqlDev.IDECallBacks;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2
{
    class NinjectPluginModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDECallbacks>().ToSelf().InSingletonScope();
            Bind<IIDECallbacks>().ToMethod(x => x.Kernel.Get<IDECallbacks>());
            Bind<Menu>().ToSelf().InSingletonScope();
            Bind<IMenu>().ToMethod(x => x.Kernel.Get<Menu>());
        }
    }
}
