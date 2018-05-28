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

        public IContainerExtension ContainerExtension { get; }

        private readonly RepositoryBootstrapper _repositoryBootstrapper;

        public Bootstrapper(bool useFileProxies)
        {
            UseFileProxies = useFileProxies;
            //UseFileProxies = true;

            ContainerExtension = new UnityContainerExtension(new UnityContainer());

            _repositoryBootstrapper = new RepositoryBootstrapper(ContainerExtension);
        }

        public void Run(bool runWithDefaultConfiguration = true)
        {
            try
            {
                //ILogger
                var fakeLogger = A.Fake<ILogger>();
                ContainerExtension.RegisterInstance(fakeLogger);
                
                //IAccountService
                ContainerExtension.RegisterInstance(IntegrationTestSettings.Get());
                var fakeAccountService = A.Fake<IAccountService>();
                var restApiAccount = ContainerExtension.Resolve<IntegrationTestSettings>().RestApiAccount;
                A.CallTo(() => fakeAccountService.RestApiAccount).Returns(restApiAccount);
                ContainerExtension.RegisterInstance(fakeAccountService);
                
                //ITableObjectGenerator
                ContainerExtension.Register<ITableObjectGenerator, TableObjectGenerator>();
                
                //ITokenService
                ContainerExtension.RegisterSingleton<ITokenService, TokenService>();
                ContainerExtension.Resolve<IAccountService>().Init(ContainerExtension.Resolve<IntegrationTestSettings>().RestApiAccount);
                
                //IPhpCrudApiService
                ContainerExtension.RegisterSingleton<IPhpCrudApiService, PhpCrudApiService>();

                //IPasswordService
                ContainerExtension.RegisterSingleton<IPasswordService, PasswordService>();
                
                
                ContainerExtension.RegisterSingleton<IJsonFileFactory, JsonFileFactory>();
                
                _repositoryBootstrapper.RegisterRepositories(UseFileProxies); //always set in construtor for notifying unit tests
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }

        protected override void OnInitialized()
        {
            
        }
    }
}