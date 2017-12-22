using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class PasswordPageViewModel : ViewModelBase
    {
        public int MinPasswordLength => 8;

        private User _user;

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private readonly IPasswordService _passwordService;
        private readonly IProxy<User> _usersProxy;

        #region Properties

        #region Password1

        private string _password1;

        public string Password1
        {
            get => _password1;
            set
            {
                SetProperty(ref _password1, value);
                var length = (_password1 ?? string.Empty).Length;
                IsValidPassword1 = length >= MinPasswordLength;
                Password1Length = length;
                (SaveAndContinueCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Password1Length

        private int _password1Length;

        public int Password1Length
        {
            get => _password1Length;
            set => SetProperty(ref _password1Length, value);
        }

        #endregion

        #region IsValidPassword1

        private bool _isValidPassword1;

        public bool IsValidPassword1
        {
            get => _isValidPassword1;
            set => SetProperty(ref _isValidPassword1, value);
        }

        #endregion


        #region Password2

        private string _password2;

        public string Password2
        {
            get => _password2;
            set
            {
                SetProperty(ref _password2, value);
                var length = (_password2 ?? string.Empty).Length;
                IsValidPassword2 = length >= MinPasswordLength;
                Password2Length = length;

                (SaveAndContinueCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Password2Length

        private int _password2Length;

        public int Password2Length
        {
            get => _password2Length;
            set => SetProperty(ref _password2Length, value);
        }

        #endregion

        #region IsValidPassword2

        private bool _isValidPassword2;

        public bool IsValidPassword2
        {
            get => _isValidPassword2;
            set => SetProperty(ref _isValidPassword2, value);
        }

        #endregion

        #endregion

        #region Commands

        #region SaveAndContinue

        public ICommand SaveAndContinueCommand { get; }

        private async void OnSaveAndContinue()
        {
            var saltedPasswordHash = await _passwordService.HashPasswordAsync(Password1, App.AppId);

            User.Password = saltedPasswordHash;

            await _usersProxy.UpdateAsync(User);

            await _navigationService.NavigateAsync("StartPage");
        }

        private bool CanSaveAndContinue()
        {
            return _isValidPassword1 && _isValidPassword2 && Password1 == Password2;
        }

        #endregion

        #endregion

        public PasswordPageViewModel(INavigationService navigationService, IPasswordService passwordService, IProxy<User> usersProxy) : base(navigationService)
        {
            Title = "Passwort";

            _passwordService = passwordService;
            _usersProxy = usersProxy;
            SaveAndContinueCommand = new DelegateCommand(OnSaveAndContinue, CanSaveAndContinue);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            var userFromNavigationParameters = (User) parameters["User"];

            User = userFromNavigationParameters;
        }
    }
}
