using System;
using System.Net;
using AVF.MemberManagement.PortableLibrary.Services;
using AVF.MemberManagement.Services;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Services;
using Prism.Unity;
using AVF.MemberManagement.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;
using Xamarin.Forms;

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
            ILoggerFactory loggerFactory = new LoggerFactory();
            ILogger logger = loggerFactory.CreateLogger<App>();
            Container.RegisterInstance(loggerFactory);
            Container.RegisterInstance(logger);

            Container.RegisterInstance(new CookieContainer());

            Container.RegisterTypeForNavigation<MainPage>();
            //Container.RegisterTypeForNavigation<UserAdministrationPage>();
            //Container.RegisterTypeForNavigation<EnterPasswordPage>();
            if (Globals.UsesXamarinAuth)
            {
                Container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            }
            else
            {
                Container.RegisterInstance(Globals.AccountService);
            }

            Container.Resolve<IAccountService>().InitWithAccountStore(AppId);

            Container.RegisterType<ITokenService, TokenService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IPhpCrudApiService, PhpCrudApiService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUsersProxy, UsersProxy>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISettingsProxy, SettingsProxy>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IPasswordService, PasswordService>(new ContainerControlledLifetimeManager());
            Container.RegisterTypeForNavigation<RestApiSettingsPage>();
            Container.RegisterTypeForNavigation<StartPage>();
            Container.RegisterTypeForNavigation<PasswordPage>();
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
