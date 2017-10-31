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
        public override void Run(bool runWithDefaultConfiguration)
        {
            try
            {
                Container = new UnityContainer();
                
                //ILogger
                var fakeLogger = A.Fake<ILogger>();
                Container.RegisterInstance(fakeLogger);
                
                //IAccountService
                Container.RegisterInstance(IntegrationTestSettings.Get());
                var fakeAccountService = A.Fake<IAccountService>();
                var restApiAccount = Container.Resolve<IntegrationTestSettings>().RestApiAccount;
                A.CallTo(() => fakeAccountService.RestApiAccount).Returns(restApiAccount);
                Container.RegisterInstance(fakeAccountService);
                
                //ITableObjectGenerator
                Container.RegisterType<ITableObjectGenerator, TableObjectGenerator>();
                
                //ITokenService
                Container.RegisterType<ITokenService, TokenService>(new ContainerControlledLifetimeManager());
                Container.Resolve<IAccountService>().Init(Container.Resolve<IntegrationTestSettings>().RestApiAccount);
                
                //IPhpCrudApiService
                Container.RegisterType<IPhpCrudApiService, PhpCrudApiService>(new ContainerControlledLifetimeManager());

                //IPasswordService
                Container.RegisterType<IPasswordService, PasswordService>(new ContainerControlledLifetimeManager());
                
                
                RegisterRepositories();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void RegisterRepositories()
        {
            Container.RegisterType<IProxy<Beitragsklasse>, Proxy<TblBeitragsklassen, Beitragsklasse>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Beitragsklasse>, Repository<Beitragsklasse>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Familienrabatt>, Proxy<TblFamilienrabatte, Familienrabatt>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Familienrabatt>, Repository<Familienrabatt>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Graduierung>, Proxy<TblGraduierungen, Graduierung>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Graduierung>, Repository<Graduierung>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Kurs>, Proxy<TblKurse, Kurs>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Kurs>, Repository<Kurs>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Mitglied>, Proxy<TblMitglieder, Mitglied>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Mitglied>, Repository<Mitglied>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Pruefung>, Proxy<TblPruefungen, Pruefung>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Pruefung>, Repository<Pruefung>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxyBase<Setting, string>, ProxyBase<TblSettings, Setting, string>>(
                new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepositoryBase<Setting, string>, CachedSettingsRepository>(
                new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Stundensatz>, Proxy<TblStundensaetze, Stundensatz>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Stundensatz>, Repository<Stundensatz>>(new ContainerControlledLifetimeManager());
            
            Container.RegisterType<IProxy<Test>, Proxy<TblTests, Test>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Test>, Repository<Test>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<TrainerErnennung>, Proxy<TblTrainerErnennungen, TrainerErnennung>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<TrainerErnennung>, Repository<TrainerErnennung>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<TrainerStufe>, Proxy<TblTrainerStufen, TrainerStufe>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<TrainerStufe>, Repository<TrainerStufe>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Training>, Proxy<TblTrainings, Training>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Training>, Repository<Training>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<TrainingsTeilnahme>, Proxy<TblTrainingsTeilnahmen, TrainingsTeilnahme>>();
            Container.RegisterType<IRepository<TrainingsTeilnahme>, Repository<TrainingsTeilnahme>>();
            
            Container.RegisterType<IProxy<User>, Proxy<TblUsers, User>>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<IUsersProxy, UsersProxy>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<User>, CachedRepository<User>>();

            Container.RegisterType<IProxy<Wochentag>, Proxy<TblWochentage, Wochentag>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Wochentag>, Repository<Wochentag>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Wohnung>, Proxy<TblWohnungen, Wohnung>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Wohnung>, Repository<Wohnung>>(new ContainerControlledLifetimeManager());

            Container
                .RegisterType<IProxy<ZuschlagKindertraining>, Proxy<TblZuschlaegeKindertraining, ZuschlagKindertraining>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<ZuschlagKindertraining>, Repository<ZuschlagKindertraining>>(new ContainerControlledLifetimeManager());
        }
    }
}