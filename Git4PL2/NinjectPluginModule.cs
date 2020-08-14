using Git4PL2.Abstarct;
using Git4PL2.Git;
using Git4PL2.Git.Abstract;
using Git4PL2.IDE;
using Git4PL2.Plugin;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Model;
using Git4PL2.Plugin.Processes;
using Moq;
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
            
            Bind<IWarnings>().To<Warnings>();

            var mock = new Mock<IWarnings>();
            mock.Setup(x => x.IsBranchUnexsepted(It.IsAny<string>(), It.IsAny<bool>())).Returns(false);
            mock.Setup(x => x.IsServerUnexsepted(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);
            Bind<IWarnings>().ToConstant(mock.Object);

            Bind<IDiffText>().To<DiffText>();
            Bind<IGitAPI>().To<GitAPI>();
        }
    }
}
