using System.Globalization;
using System.Threading;
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

        protected override async void OnInitialized()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            InitializeComponent();

            await NavigationService.NavigateAsync("LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(Globals.AccountService);

            containerRegistry.Register<IProxy<User>, Proxy<TblUsers, User>>();
            containerRegistry.RegisterSingleton<IRepository<User>, Repository<User>>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<CalenderPage, CalenderPageViewModel>();
            containerRegistry.RegisterForNavigation<CourseSelectionPage, CourseSelectionPageViewModel>();
            containerRegistry.RegisterForNavigation<TrainingEditPage, TrainingEditPageViewModel>();
            containerRegistry.RegisterForNavigation<ParticipantsSelectionPage, ParticipantsSelectionPageViewModel>();
            containerRegistry.RegisterForNavigation<SaveParticipantsPage, SaveParticipantsPageViewModel>();
            containerRegistry.RegisterForNavigation<SaveStatusPage, SaveStatusPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
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
