using System.Globalization;
using System.Threading;
using Prism;
using Prism.Ioc;
using AVF.CourseParticipation.ViewModels;
using AVF.CourseParticipation.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<CalenderPage, CalenderPageViewModel>();
            containerRegistry.RegisterForNavigation<CourseSelectionPage, CourseSelectionPageViewModel>();
            containerRegistry.RegisterForNavigation<TrainingEditPage, TrainingEditPageViewModel>();
            containerRegistry.RegisterForNavigation<ParticipantsSelectionPage, ParticipantsSelectionPageViewModel>();
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
