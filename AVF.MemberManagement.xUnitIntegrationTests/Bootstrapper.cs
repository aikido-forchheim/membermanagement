using System;
using AVF.MemberManagement.Factories;
using AVF.MemberManagement.PortableLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Factories;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Repositories;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;
using Prism.Ioc;
using Prism.Unity;
using Unity;
using Unity.Lifetime;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class Bootstrapper : PrismApplication
    {
        public static bool UseFileProxies { get; private set; }

        private RepositoryBootstrapper _repositoryBootstrapper;

        public IContainerRegistry ContainerRegistry { get; private set; }

        public Bootstrapper(bool useFileProxies)
        {
            UseFileProxies = useFileProxies;
            //UseFileProxies = true;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ContainerRegistry = containerRegistry;

            _repositoryBootstrapper = new RepositoryBootstrapper(containerRegistry);

            try
            {
                //ILogger
                var fakeLogger = A.Fake<ILogger>();
                containerRegistry.RegisterInstance(fakeLogger);

                //IAccountService
                containerRegistry.RegisterInstance(IntegrationTestSettings.Get());
                var fakeAccountService = A.Fake<IAccountService>();
                var restApiAccount = Container.Resolve<IntegrationTestSettings>().RestApiAccount;
                A.CallTo(() => fakeAccountService.RestApiAccount).Returns(restApiAccount);
                containerRegistry.RegisterInstance(fakeAccountService);

                //ITableObjectGenerator
                containerRegistry.Register<ITableObjectGenerator, TableObjectGenerator>();

                //ITokenService
                containerRegistry.RegisterSingleton<ITokenService, TokenService>();
                Container.Resolve<IAccountService>().Init(Container.Resolve<IntegrationTestSettings>().RestApiAccount);

                //IPhpCrudApiService
                containerRegistry.RegisterSingleton<IPhpCrudApiService, PhpCrudApiService>();

                //IPasswordService
                containerRegistry.RegisterSingleton<IPasswordService, PasswordService>();


                containerRegistry.RegisterSingleton<IJsonFileFactory, JsonFileFactory>();

                _repositoryBootstrapper.RegisterRepositories(UseFileProxies); //always set in construtor for notifying unit tests
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override void OnInitialized()
        {

        }
    }
}