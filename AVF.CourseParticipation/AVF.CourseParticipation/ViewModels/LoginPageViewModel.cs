using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Views;
using Prism.AppModel;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
	public class LoginPageViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        private const string LastLoggedInUsernameKey = "LastLoggedInUsername";
        private const string LastLoggedInUsernameDefaultValue = "";

	    private string _username;

	    public string Username
	    {
	        get => _username;
	        set => SetProperty(ref _username, value);
	    }

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            LoginCommand = new DelegateCommand(Login, CanLogin);
            OpenSettingsCommand = new DelegateCommand(OpenSettings, CanOpenSettings);
        }

        private bool CanOpenSettings()
        {
            return true;
        }

        private void OpenSettings()
        {
            NavigationService.NavigateAsync(nameof(SettingsPage));
        }

        private bool CanLogin()
        {
            return true;
        }

        private async void Login()
        {
            EnsurePropertyLastLoggedInUsername();
            Prism.PrismApplicationBase.Current.Properties[LastLoggedInUsernameKey] = Username;

            //await NavigationService.NavigateAsync("NavigationPage/CalenderPage", null, true);
            await NavigationService.NavigateAsync("/NavigationPage/CalenderPage");
        }

	    public override void OnNavigatingTo(INavigationParameters parameters)
        {
            EnsurePropertyLastLoggedInUsername();
            Username = Prism.PrismApplicationBase.Current.Properties[LastLoggedInUsernameKey].ToString();
        }

        private static void EnsurePropertyLastLoggedInUsername()
        {
            EnsureProperty(LastLoggedInUsernameKey, LastLoggedInUsernameDefaultValue);
        }

        private static void EnsureProperty(string key, string defaultValue)
        {
            if (!Prism.PrismApplicationBase.Current.Properties.ContainsKey(key))
            {
                Prism.PrismApplicationBase.Current.Properties.Add(key, defaultValue);
            }
        }
    }
}
