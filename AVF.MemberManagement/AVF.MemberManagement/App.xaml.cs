using System.Diagnostics;
using System.Threading.Tasks;
using AVF.MemberManagement.Factories;
using AVF.MemberManagement.PortableLibrary.Services;
using AVF.MemberManagement.Services;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;
using Prism.Unity;
using AVF.MemberManagement.Views;
using Microsoft.Extensions.Logging;
using Prism;
using Prism.Ioc;
using Unity.Lifetime;
using Xamarin.Forms;

namespace AVF.MemberManagement
{
    public partial class App
    {
        private RepositoryBootstrapper _repositoryBootstrapper;

        public const string AppId = "10bc9068-17ac-4f0f-a596-7fdfe20bc9f4";

        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }

        public App()
        {
            
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Globals.Idiom = (Idiom)(int)Device.Idiom;

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
            //await NavigationService.NavigateAsync("MainMasterDetailPage");
        }

        protected override void RegisterTypes(IContainerRegistry Container)
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
                Container.RegisterSingleton<IAccountService, AccountService>();
            }
            else
            {
                Container.RegisterInstance(Globals.AccountService);
            }

            this.Container.Resolve<IAccountService>().InitWithAccountStore(AppId);
            
            //ITokenService
            Container.RegisterSingleton<ITokenService, TokenService>();
            
            //IPhpCrudApiService
            Container.RegisterSingleton<IPhpCrudApiService, PhpCrudApiService>();
            
            //IPasswordService
            Container.RegisterSingleton<IPasswordService, PasswordService>();

            Container.RegisterSingleton<IKursModelService, KursModelService>();


            Container.RegisterSingleton<IJsonFileFactory, JsonFileFactory>();

            //_repositoryBootstrapper.RegisterRepositories(false);
            //Globals.UseFileProxies = false;
            //Globals.UseFileProxies = true;
            _repositoryBootstrapper.RegisterRepositories(Globals.UseFileProxies);

            //RefreshCache().Wait();
            //RefreshCache(); //UWP

            Container.RegisterForNavigation<MainMasterDetailPage>();
            Container.RegisterForNavigation<MenuPage>();
            Container.RegisterForNavigation<NavigationPage>();
            Container.RegisterForNavigation<MainPage>();
            Container.RegisterForNavigation<RestApiSettingsPage>();
            Container.RegisterForNavigation<AdvancedSettingsPage>();
            Container.RegisterForNavigation<PasswordPage>();
            Container.RegisterForNavigation<StartPage>();
            Container.RegisterForNavigation<DaySelectionPage>();
            Container.RegisterForNavigation<KursSelectionPage>();
            Container.RegisterForNavigation<EditTrainingPage>();
            Container.RegisterForNavigation<EnterParticipantsPage>();
            Container.RegisterForNavigation<EnterParticipantsTabletPage>();
            Container.RegisterForNavigation<SaveParticipantsPage>();
            Container.RegisterForNavigation<SelectTrainerPage>();
        }

        // ReSharper disable once UnusedMember.Local
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
