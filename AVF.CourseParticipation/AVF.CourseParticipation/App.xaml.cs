using System.Globalization;
using System.Threading;
using AVF.CourseParticipation.Models;
using AVF.CourseParticipation.Services;
using Prism;
using Prism.Ioc;
using AVF.CourseParticipation.ViewModels;
using AVF.CourseParticipation.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Repositories;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.StandardLibrary;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prism.Unity;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AVF.CourseParticipation
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        public const string AppId = "10bc9068-17ac-4f0f-a596-7fdfe20bc9f4";

        protected override async void OnInitialized()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            InitializeComponent();

            await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterInstance(Globals.AccountService);

            //ILogger
            ILoggerFactory loggerFactory = new LoggerFactory();

#pragma warning disable 618
            loggerFactory.AddConsole();
#pragma warning restore 618

            //loggerFactory.AddSentry();

            ILogger logger = loggerFactory.CreateLogger<App>();
            containerRegistry.RegisterInstance(loggerFactory);
            containerRegistry.RegisterInstance(logger);

            IOptions<AppOptions> appOptions = new OptionsWrapper<AppOptions>(new AppOptions());
            containerRegistry.RegisterInstance(appOptions);

            containerRegistry.Register<ITokenService, TokenService>();
            containerRegistry.RegisterSingleton<IAccountService, AccountService>();
            containerRegistry.Register<IPhpCrudApiService, PhpCrudApiService>();
            containerRegistry.Register<IPasswordService, PasswordService>();

            containerRegistry.Register<IProxyBase<Setting, string>, ProxyBase<TblSettings, Setting, string>>();
            containerRegistry.Register<IProxy<User>, Proxy<TblUsers, User>>();
            containerRegistry.Register<IProxy<Mitglied>, Proxy<TblMitglieder, Mitglied>>();
            containerRegistry.Register<IProxy<Kurs>, Proxy<TblKurse, Kurs>>();
            containerRegistry.Register<IProxy<TrainerErnennung>, Proxy<TblTrainerErnennungen, TrainerErnennung>>();

            containerRegistry.RegisterSingleton<IRepository<User>, Repository<User>>();
            containerRegistry.RegisterSingleton<IRepository<Mitglied>, Repository<Mitglied>>();
            containerRegistry.RegisterSingleton<IRepository<Kurs>, Repository<Kurs>>();
            containerRegistry.RegisterSingleton<IRepository<TrainerErnennung>, Repository<TrainerErnennung>>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<CalenderPage, CalenderPageViewModel>();
            containerRegistry.RegisterForNavigation<CourseSelectionPage, CourseSelectionPageViewModel>();
            containerRegistry.RegisterForNavigation<TrainingEditPage, TrainingEditPageViewModel>();
            containerRegistry.RegisterForNavigation<ParticipantsSelectionPage, ParticipantsSelectionPageViewModel>();
            containerRegistry.RegisterForNavigation<SaveParticipantsPage, SaveParticipantsPageViewModel>();
            containerRegistry.RegisterForNavigation<SaveStatusPage, SaveStatusPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<NewPasswordPage, NewPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<TrainerSelectionPage, TrainerSelectionPageViewModel>();
        }
        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override async void OnSleep()
        {
            base.OnSleep();
            await Current.SavePropertiesAsync();
        }
    }
}
