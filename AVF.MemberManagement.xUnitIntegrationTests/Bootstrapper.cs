using System;
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
using Prism.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class Bootstrapper : UnityBootstrapper
    {
        private readonly bool _useFileProxies;

        private readonly IUnityContainer _container;

        private readonly RepositoryBootstrapper _repositoryBootstrapper;

        public Bootstrapper(bool useFileProxies)
        {
            _useFileProxies = useFileProxies;

            _container = new UnityContainer();

            Container = _container;

            _repositoryBootstrapper = new RepositoryBootstrapper(_container);
        }

        public override void Run(bool runWithDefaultConfiguration)
        {
            try
            {
                //ILogger
                var fakeLogger = A.Fake<ILogger>();
                _container.RegisterInstance(fakeLogger);
                
                //IAccountService
                _container.RegisterInstance(IntegrationTestSettings.Get());
                var fakeAccountService = A.Fake<IAccountService>();
                var restApiAccount = _container.Resolve<IntegrationTestSettings>().RestApiAccount;
                A.CallTo(() => fakeAccountService.RestApiAccount).Returns(restApiAccount);
                _container.RegisterInstance(fakeAccountService);
                
                //ITableObjectGenerator
                _container.RegisterType<ITableObjectGenerator, TableObjectGenerator>();
                
                //ITokenService
                _container.RegisterType<ITokenService, TokenService>(new ContainerControlledLifetimeManager());
                _container.Resolve<IAccountService>().Init(_container.Resolve<IntegrationTestSettings>().RestApiAccount);
                
                //IPhpCrudApiService
                _container.RegisterType<IPhpCrudApiService, PhpCrudApiService>(new ContainerControlledLifetimeManager());

                //IPasswordService
                _container.RegisterType<IPasswordService, PasswordService>(new ContainerControlledLifetimeManager());
                
                
                _repositoryBootstrapper.RegisterRepositories(_useFileProxies);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}