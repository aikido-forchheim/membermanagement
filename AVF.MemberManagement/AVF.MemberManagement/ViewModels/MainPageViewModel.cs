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
    public partial class MainPageViewModel : BindableBase
    {
        public ICommand SettingsCommand { get; }
        public ICommand StartCommand { get; }

        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IUsersProxy _usersProxy;
        private readonly IPasswordService _passwordService;

        private DateTime _lastUserRequestTime;

        public bool IsRestApiAccountSet => _accountService.IsRestApiAccountSet;
        
        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);

                var userRequestTime = DateTime.Now;

                _lastUserRequestTime = userRequestTime;

                var userRequestTask = CheckUsernameWithTimestampAsync(value, userRequestTime);

                userRequestTask.ContinueWith(getServerUserTask =>
                {
                    if (getServerUserTask.Result.RequestTime >= _lastUserRequestTime)
                    {
                        CheckCanEnterPassword(getServerUserTask.IsFaulted
                            ? null
                            : getServerUserTask.Result.User);
                    }
                });
            }
        }

        private async Task<UserRequest> CheckUsernameWithTimestampAsync(string username, DateTime requestTime)
        {
            var userRequest = new UserRequest
            {
                RequestTime = requestTime,
                User = await _usersProxy.GetUserAsync(username)
            };

            return userRequest;
        }

        public User ServerUser { get; private set; }

        private void CheckCanEnterPassword(User serverUser)
        {
            if (serverUser == null || serverUser.UserId == 0)
            {
                CanEnterPassword = false;
                ServerUser = null;
            }
            else
            {
                CanEnterPassword = true;
                ServerUser = serverUser;
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

                _passwordService.IsValidAsync(Password, ServerUser.Password, App.AppId)
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
            return IsRestApiAccountSet && ServerUser != null && _isPasswordValid;
        }
    }

}
