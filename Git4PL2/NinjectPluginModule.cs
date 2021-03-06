﻿using Git4PL2.Abstarct;
using Git4PL2.Git;
using Git4PL2.Git.Abstract;
using Git4PL2.IDE;
using Git4PL2.IDE.Abstarct;
using Git4PL2.IDE.SQL;
using Git4PL2.Plugin;
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Diff;
using Git4PL2.Plugin.Commands;
using Git4PL2.Plugin.Settings;
using Git4PL2.Plugin.TeamCoding;
using Git4PL2.Plugin.TeamCoding.FileProvider;
using Moq;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Git4PL2.Plugin.WPF.Model;

namespace Git4PL2
{
    class NinjectPluginModule : NinjectModule
    {
        public override void Load()
        {
            Bind<PluginSettingsStorage>().ToSelf().InSingletonScope();
            Bind<IPluginSettingsStorage>().ToMethod(x => x.Kernel.Get<PluginSettingsStorage>());
            Bind<ISettings>().ToMethod(x => x.Kernel.Get<Settings>());

            Bind<CallbackManager>().ToSelf().InSingletonScope();
            Bind<ICallbackManager>().ToMethod(x => x.Kernel.Get<CallbackManager>());

            Bind(typeof(ISQLQueryExecute<>)).To(typeof(SQLQueryExecute<>));

            Bind<Menu>().ToSelf().InSingletonScope();
            Bind<IMenu>().ToMethod(x => x.Kernel.Get<Menu>());

            Bind<IPlsqlCodeFormatter>().To<PlsqlCodeFormatter>();
            Bind<IPluginCommands>().To<PluginCommands>();

#if IDESTUB
            var mockIDEProvider = GetIDEStub();
            Bind<IIDEProvider>().ToConstant(mockIDEProvider);
#else
            Bind<IIDEProvider>().To<IDEProvider>();
#endif

            Bind<IWarnings>().To<Warnings>();

            // Тест предупреждений
            /*var mock = new Mock<IWarnings>();
            mock.Setup(x => x.IsBranchUnexsepted(It.IsAny<string>(), It.IsAny<bool>())).Returns(false);
            mock.Setup(x => x.IsServerUnexsepted(It.IsAny<string>(), It.IsAny<bool>())).Returns(true);
            Bind<IWarnings>().ToConstant(mock.Object);*/

            Bind<IDiffText>().To<DiffText>();
            Bind<IGitAPI>().To<GitAPI>();

            /*var TeamCodingProvider = GetMockProvider();
            Bind<ITeamCodingProvider>().ToConstant(TeamCodingProvider);*/



            Bind<ITeamCodingProviderChecks>().To<TeamCodingProviderChecks>();
            Bind<ITeamCodingChecks>().To<TeamCodingChecks>();
        }

        private IIDEProvider GetIDEStub()
        {
            Mock<IIDEProvider> mock = new Mock<IIDEProvider>();

            // Запрос имени сервера
            mock.Setup(x => x.SQLQueryExecute<DummyString>(It.IsRegex("global_name")))
                .Returns(new List<DummyString>() { new DummyString() { Value = "STUB SERVER" }});

            // Запрос Dicti
            mock.Setup(x => x.SQLQueryExecute<Dicti>(It.IsAny<string>()))
                .Returns(new List<Dicti>()
                {
                    new Dicti() {
                        Active = "A",
                        Code   = "1",
                        FullName = "Stub Fullname",
                        ConstName = "StubConstname", 
                        Isn = 123, 
                        Level = 1, 
                        ParentIsn = 123, 
                        ShortName = "Stub shortname"
                    }
                });

            return mock.Object;
        }

        private ITeamCodingProvider GetMockProvider()
        {
            Mock<ICheckOutObject> mock = new Mock<ICheckOutObject>();
            mock.Setup(x => x.Login).Returns("bobylev");
            mock.Setup(x => x.ServerName).Returns("XE");
            mock.Setup(x => x.ObjectOwner).Returns("SIA");
            mock.Setup(x => x.ObjectName).Returns("LDOCTOP");
            mock.Setup(x => x.ObjectType).Returns("PACKAGE BODY");
            mock.Setup(x => x.CheckoutDate).Returns(DateTime.Now);
            var CO = mock.Object;

            mock = new Mock<ICheckOutObject>();
            mock.Setup(x => x.Login).Returns("Gareev");
            mock.Setup(x => x.ServerName).Returns("XE");
            mock.Setup(x => x.ObjectOwner).Returns("SIA");
            mock.Setup(x => x.ObjectName).Returns("LDOCTOP");
            mock.Setup(x => x.ObjectType).Returns("PACKAGE");
            mock.Setup(x => x.CheckoutDate).Returns(DateTime.Now.AddDays(-1));
            var CO2 = mock.Object;

            var teamCodingMock = new Mock<ITeamCodingProvider>();
            teamCodingMock.Setup(x => x.GetCheckOutObjectsList()).Returns(new[] { CO, CO2 });
            teamCodingMock.Setup(x => x.CheckIn(It.IsAny<string>(), It.IsAny<IDbObject>(), out It.Ref<string>.IsAny)).Returns(true);
            teamCodingMock.Setup(x => x.CheckOut(It.IsAny<string>(), It.IsAny<IDbObject>(), out It.Ref<string>.IsAny)).Returns(true);

            return teamCodingMock.Object;
        }
    }
}
