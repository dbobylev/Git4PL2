using Git4PL2.Abstarct;
using Git4PL2.Git;
using Git4PL2.Git.Abstract;
using Git4PL2.IDE;
using Git4PL2.Plugin;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Model;
using Git4PL2.Plugin.Processes;
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
            Bind<CallbackManager>().ToSelf().InSingletonScope();
            Bind<ICallbackManager>().ToMethod(x => x.Kernel.Get<CallbackManager>());

            Bind<Menu>().ToSelf().InSingletonScope();
            Bind<IMenu>().ToMethod(x => x.Kernel.Get<Menu>());

            Bind<IPlsqlCodeFormatter>().To<PlsqlCodeFormatter>();
            Bind<IPluginCommands>().To<PluginCommands>();
            Bind<IIDEProvider>().To<IDEProvider>();

            Bind<IGitAPI>().To<GitAPI>();
            Bind<IWarnings>().To<Warnings>();

        }
    }
}
