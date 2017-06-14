using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.Views;

namespace AVF.MemberManagement.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        public ICommand SettingsCommand { get; }
        public ICommand StartCommand { get; }

        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IUsersProxy _usersProxy;
        private readonly IPasswordService _passwordService;

        public bool IsRestApiAccountSet => _accountService.IsRestApiAccountSet;
        
        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);

                _usersProxy.GetUserAsync(value).ContinueWith(getServerUserTask => CheckCanEnterPassword(getServerUserTask.IsFaulted ? null : getServerUserTask.Result));
            }
        }

        private User _serverUser;

        private void CheckCanEnterPassword(User serverUser)
        {
            if (serverUser == null)
            {
                CanEnterPassword = false;
                _serverUser = null;
            }
            else
            {
                CanEnterPassword = true;
                _serverUser = serverUser;
            }

            ((DelegateCommand)StartCommand).RaiseCanExecuteChanged();
        }

        private bool _canEnterPassword;

        public bool CanEnterPassword
        {
            get => _canEnterPassword;
            set => SetProperty(ref _canEnterPassword, value);
        }

        private bool _isPasswordValid;

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);

                _passwordService.IsValidAsync(Password, _serverUser.Password, App.AppId)
                    .ContinueWith(isPasswordValidTask =>
                    {
                        _isPasswordValid = !isPasswordValidTask.IsFaulted && isPasswordValidTask.Result;
                        ((DelegateCommand) StartCommand).RaiseCanExecuteChanged();
                    });
            }
        }

        public MainPageViewModel(IAccountService accountService, INavigationService navigationService, IUsersProxy usersProxy, IPasswordService passwordService)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            _usersProxy = usersProxy;
            _passwordService = passwordService;

            SettingsCommand = new DelegateCommand(OnSettings, CanSettings);
            StartCommand = new DelegateCommand(OnStart, CanStart);
        }

        private void OnSettings()
        {
            _navigationService.NavigateAsync(nameof(RestApiSettingsPage));
        }

        private static bool CanSettings()
        {
            return true;
        }

        private void OnStart()
        {
            //_navigationService.NavigateAsync(nameof(StartPage));
        }

        private bool CanStart()
        {
            return IsRestApiAccountSet && _serverUser != null && _isPasswordValid;
        }
    }

}
