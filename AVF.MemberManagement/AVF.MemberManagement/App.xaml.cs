using System.Net;
using AVF.MemberManagement.PortableLibrary.Services;
using AVF.MemberManagement.Services;
using AVF.MemberManagement.StandardLibrary.Archive;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Repositories;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Unity;
using AVF.MemberManagement.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement
{
    public partial class App : PrismApplication
    {
        public const string AppId = "10bc9068-17ac-4f0f-a596-7fdfe20bc9f4";

        public App(IPlatformInitializer initializer = null) : base(initializer)
        {

        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync(nameof(MainPage));
        }

        protected override void RegisterTypes()
        {
            //ILogger
            ILoggerFactory loggerFactory = new LoggerFactory();
            ILogger logger = loggerFactory.CreateLogger<App>();
            Container.RegisterInstance(loggerFactory);
            Container.RegisterInstance(logger);
            
            //IAccountService
            if (Globals.UsesXamarinAuth)
            {
                Container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            }
            else
            {
                Container.RegisterInstance(Globals.AccountService);
            }
            Container.Resolve<IAccountService>().InitWithAccountStore(AppId);
            
            //ITokenService
            Container.RegisterType<ITokenService, TokenService>(new ContainerControlledLifetimeManager());
            
            //IPhpCrudApiService
            Container.RegisterType<IPhpCrudApiService, PhpCrudApiService>(new ContainerControlledLifetimeManager());
            
            //IPasswordService
            Container.RegisterType<IPasswordService, PasswordService>(new ContainerControlledLifetimeManager());

            RegisterRepositories();

            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<RestApiSettingsPage>();
            Container.RegisterTypeForNavigation<PasswordPage>();
            Container.RegisterTypeForNavigation<StartPage>();
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
            Container.RegisterType<IRepository<User>, CachedRepository<User>>();

            Container.RegisterType<IProxy<Wochentag>, Proxy<TblWochentage, Wochentag>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Wochentag>, Repository<Wochentag>>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IProxy<Wohnung>, Proxy<TblWohnungen, Wohnung>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<Wohnung>, Repository<Wohnung>>(new ContainerControlledLifetimeManager());

            Container
                .RegisterType<IProxy<ZuschlagKindertraining>, Proxy<TblZuschlaegeKindertraining, ZuschlagKindertraining>>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IRepository<ZuschlagKindertraining>, Repository<ZuschlagKindertraining>>(new ContainerControlledLifetimeManager());
        }

        //protected override void OnStart()
        //{
        //	// Handle when your app starts
        //}

        //protected override void OnSleep()
        //{
        //	// Handle when your app sleeps
        //}

        //protected override void OnResume()
        //{
        //	// Handle when your app resumes
        //}
    }
}
