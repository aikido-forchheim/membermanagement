using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using AVF.MemberManagement.Factories;
using AVF.MemberManagement.PortableLibrary.Services;
using AVF.MemberManagement.Services;
using AVF.MemberManagement.StandardLibrary.Archive;
using AVF.MemberManagement.StandardLibrary.Enums;
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
using Xamarin.Forms;

namespace AVF.MemberManagement
{
    public partial class App : PrismApplication
    {
        private RepositoryBootstrapper _repositoryBootstrapper;

        public const string AppId = "10bc9068-17ac-4f0f-a596-7fdfe20bc9f4";

        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Globals.Idiom = (Idiom)(int)Device.Idiom;

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes()
        {
            _repositoryBootstrapper = new RepositoryBootstrapper(Container);

            Container.RegisterInstance<IRepositoryBootstrapper>(_repositoryBootstrapper);

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

            Container.RegisterType<IKursModelService, KursModelService>(new ContainerControlledLifetimeManager());


            Container.RegisterType<IJsonFileFactory, JsonFileFactory>(new ContainerControlledLifetimeManager());

            //_repositoryBootstrapper.RegisterRepositories(false);
            //Globals.UseFileProxies = false;
            //Globals.UseFileProxies = true;
            _repositoryBootstrapper.RegisterRepositories(Globals.UseFileProxies);

            //RefreshCache().Wait();
            //RefreshCache(); //UWP

            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<RestApiSettingsPage>();
            Container.RegisterTypeForNavigation<AdvancedSettingsPage>();
            Container.RegisterTypeForNavigation<PasswordPage>();
            Container.RegisterTypeForNavigation<StartPage>();
            Container.RegisterTypeForNavigation<DaySelectionPage>();
            Container.RegisterTypeForNavigation<KursSelectionPage>();
            Container.RegisterTypeForNavigation<EditTrainingPage>();
            Container.RegisterTypeForNavigation<EnterParticipantsPage>();
            Container.RegisterTypeForNavigation<EnterParticipantsTabletPage>();
            Container.RegisterTypeForNavigation<SaveParticipantsPage>();
        }

        private async Task RefreshCache()
        {
            var factory = Container.Resolve<IJsonFileFactory>();
            var fileList = await factory.RefreshFileCache();
            
            Debug.WriteLine("Cached files count:" + fileList.Count);
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
