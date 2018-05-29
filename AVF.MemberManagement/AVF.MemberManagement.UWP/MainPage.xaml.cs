using Windows.UI.Core;
using Prism.Unity;
using Prism;
using Prism.Ioc;
using Unity;

namespace AVF.MemberManagement.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new AVF.MemberManagement.App(new UwpInitializer()));

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Visible;
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }

}
