using System;
using AVF.MemberManagement.PortableLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class Bootstrapper : UnityBootstrapper
    {
        public override void Run(bool runWithDefaultConfiguration)
        {
            try
            {
                Container = new UnityContainer();

                var fakeLogger = A.Fake<ILogger>();

                Container.RegisterInstance(fakeLogger);
                Container.RegisterInstance(IntegrationTestSettings.Get());
            
                var fakeAccountService = A.Fake<IAccountService>();
                var restApiAccount = Container.Resolve<IntegrationTestSettings>().RestApiAccount;
                A.CallTo(() => fakeAccountService.RestApiAccount).Returns(restApiAccount);
                Container.RegisterInstance(fakeAccountService);

                Container.RegisterType<ITokenService, TokenService>(new ContainerControlledLifetimeManager());
                Container.Resolve<IAccountService>().Init(Container.Resolve<IntegrationTestSettings>().RestApiAccount);
                Container.RegisterType<IPhpCrudApiService, PhpCrudApiService>(new ContainerControlledLifetimeManager());
                Container.RegisterType<ITestProxy, TestsProxy>(new ContainerControlledLifetimeManager());
                Container.RegisterType<IUsersProxy, UsersProxy>(new ContainerControlledLifetimeManager());
                Container.RegisterType<ISettingsProxy, SettingsProxy>(new ContainerControlledLifetimeManager());
                Container.RegisterType<IPasswordService, PasswordService>(new ContainerControlledLifetimeManager());
                Container.RegisterType<IBeitragsklassenProxy, BeitragsklassenProxy>(new ContainerControlledLifetimeManager());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
