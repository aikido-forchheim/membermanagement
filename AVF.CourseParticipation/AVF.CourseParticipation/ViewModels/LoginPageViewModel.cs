using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Models;
using AVF.CourseParticipation.Views;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Services;

namespace AVF.CourseParticipation.ViewModels
{
	public class LoginPageViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly IRepository<User> _usersRepository;
        private readonly ILogger _logger;
        private readonly IPasswordService _passwordService;
        private readonly IPageDialogService _dialogService;
        private readonly IOptions<AppOptions> _appOptions;
        private readonly IRepository<Mitglied> _memberRepository;
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

        private bool _isRunning;

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public LoginPageViewModel(INavigationService navigationService, IAccountService accountService, IRepository<User> usersRepository, ILogger logger, IPasswordService passwordService, IPageDialogService dialogService, IOptions<AppOptions> appOptions, IRepository<Mitglied> memberRepository) : base(navigationService)
        {
            _accountService = accountService;
            _usersRepository = usersRepository;
            _logger = logger;
            _passwordService = passwordService;
            _dialogService = dialogService;
            _appOptions = appOptions;
            _memberRepository = memberRepository;

            _accountService.InitWithAccountStore(App.AppId);

            LoginCommand = new DelegateCommand(Login, CanLogin).ObservesProperty(() => IsRunning); ;
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
            if (IsRunning) return false;

            bool isRestApiAccountSet = _accountService.IsRestApiAccountSet;
            return isRestApiAccountSet;
        }

        private async void Login()
        {
            IsRunning = true;
            ClearLoggedInMemberId();

            EnsurePropertyLastLoggedInUsername();
            Prism.PrismApplicationBase.Current.Properties[LastLoggedInUsernameKey] = Username;

            try
            {
                var users = await _usersRepository.GetAsync();

                if (users.Any(user => user.Username == Username))
                {
                    var username = Username ?? string.Empty;

                    var user = users.Single(u => u.Username == username);

                    _appOptions.Value.User = user;
                    _appOptions.Value.UserMember = await _memberRepository.GetAsync(user.Mitgliedsnummer);

                    IsRunning = false;

                    var password = Password ?? string.Empty;

                    if (user.Password.Length < 20 && password == user.Password)
                    {
                        await _dialogService.DisplayAlertAsync("Initialpasswort gefunden", "Bitte vergeben Sie jetzt Ihr persönliches Kennwort!", "OK");

                        var navigationParameters = new NavigationParameters {{"UserId", user.Id}};
                        await NavigationService.NavigateAsync("NewPasswordPage", navigationParameters);

                        return;
                    }

                    var isValid = await _passwordService.IsValidAsync(password, user.Password, null);

                    if (isValid)
                    {
                        SetLoggedInMemberId(user);
                        await NavigationService.NavigateAsync("/NavigationPage/CalenderPage");

                        return;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogTrace(e.ToString());
            }

            await _dialogService.DisplayAlertAsync("Fehler bei der Anmeldung", "Benutzername oder Passwort falsch!", "OK");

            IsRunning = false;
        }

        public static void ClearLoggedInMemberId()
        {
            ViewModelBaseLoggedIn.LoggedInMemberId = null;
        }

        public static void SetLoggedInMemberId(UserBase user)
        {
            var loggedInMemberId = user.Mitgliedsnummer;
            ViewModelBaseLoggedIn.LoggedInMemberId = loggedInMemberId;
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
