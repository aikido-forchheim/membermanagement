using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Views;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.AppModel;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
	public class LoginPageViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly IRepository<User> _usersRepository;
        public ICommand LoginCommand { get; }
        public ICommand OpenSettingsCommand { get; }

        private const string LastLoggedInUsernameKey = "LastLoggedInUsername";
        private const string LastLoggedInUsernameDefaultValue = "";

	    private string _username;

	    public string Username
	    {
	        get => _username;
	        set
	        {
	            SetProperty(ref _username, value);
                ((DelegateCommand)OpenSettingsCommand).RaiseCanExecuteChanged();
	        }
	    }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                ((DelegateCommand)OpenSettingsCommand).RaiseCanExecuteChanged();
            }
        }

        public LoginPageViewModel(INavigationService navigationService, IAccountService accountService, IRepository<User> usersRepository) : base(navigationService)
        {
            _accountService = accountService;
            _usersRepository = usersRepository;

            _accountService.InitWithAccountStore(App.AppId);

            LoginCommand = new DelegateCommand(Login, CanLogin);
            OpenSettingsCommand = new DelegateCommand(OpenSettings, CanOpenSettings);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ((DelegateCommand)LoginCommand).RaiseCanExecuteChanged();
        }

        private bool CanOpenSettings()
        {
            if (!_accountService.IsRestApiAccountSet)
            {
                return true;
            }
            else
            {
                return Username == _accountService.RestApiAccount.Username &&
                       Password == _accountService.RestApiAccount.Password;
            }
        }

        private void OpenSettings()
        {
            NavigationService.NavigateAsync(nameof(SettingsPage));
        }

        private bool CanLogin()
        {
            bool isRestApiAccountSet = _accountService.IsRestApiAccountSet;
            return isRestApiAccountSet;
        }

        private async void Login()
        {
            EnsurePropertyLastLoggedInUsername();
            Prism.PrismApplicationBase.Current.Properties[LastLoggedInUsernameKey] = Username;

            await NavigationService.NavigateAsync("/NavigationPage/CalenderPage");
        }

	    public override void OnNavigatingTo(INavigationParameters parameters)
        {
            EnsurePropertyLastLoggedInUsername();
            //Username = Prism.PrismApplicationBase.Current.Properties[LastLoggedInUsernameKey].ToString();
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
