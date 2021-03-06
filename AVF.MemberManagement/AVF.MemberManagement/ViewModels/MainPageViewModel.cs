﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ICommand SettingsCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand NewPasswordCommand { get; }

        private readonly IAccountService _accountService;
        private readonly IRepository<User> _usersProxy;
        private readonly IPasswordService _passwordService;
        private readonly IRepository<TrainingsTeilnahme> _trainingsTeilnahmenRepository;
        private readonly IRepository<Training> _trainingRepository;
        private readonly IRepository<Mitglied> _mitgliederRepository;

        private DateTime _latestRequestTime;

        #region Properties

        #region IsRestApiAccountSet

        public bool IsRestApiAccountSet => _accountService.IsRestApiAccountSet;

        #endregion

        #region ServerUser

        private User _serverUser;

        public User ServerUser
        {
            get => _serverUser;
            set
            {
                SetProperty(ref _serverUser, value);

                HasPassword = !string.IsNullOrEmpty(_serverUser?.Password);
                IsInitialPassword = (_serverUser?.Password ?? string.Empty).Length < 20;
                IsUserInDatabaseAndHasPassword = _isUsernameInDatabase && _hasPassword;
            }
        }

        #endregion


        #region Username

        private string _username;

        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);

                var now = DateTime.Now;

                _latestRequestTime = now;

                RequestServerUserAsync(value, now).ContinueWith(task =>
                {
                    if (task.Result.RequestTime >= _latestRequestTime)
                    {
                        EnablePasswortBoxIfUsernameWasFoundOnServer(GetUserFromTask(task));
                    }
                });
            }
        }

        #endregion

        #region IsUsernameInDatabase

        private bool _isUsernameInDatabase;

        public bool IsUsernameInDatabase
        {
            get => _isUsernameInDatabase;
            set => SetProperty(ref _isUsernameInDatabase, value);
        }

        #endregion


        #region Password

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);

                IsPasswordValid = false;

                if (_isInitialPassword)
                {
                    if (Password != null && Password.Length >= 3
                        && ServerUser.Password != null && ServerUser.Password.Length >= 3
                        && Password == ServerUser.Password)
                    {
                        IsPasswordValid = true;
                    }
                }
                else
                {
                    if (ServerUser != null)
                    {
                        _passwordService.IsValidAsync(Password, ServerUser.Password, App.AppId)
                            .ContinueWith(isPasswordValidTask =>
                            {
                                IsPasswordValid = !isPasswordValidTask.IsFaulted && isPasswordValidTask.Result;
                                ((DelegateCommand) StartCommand).RaiseCanExecuteChanged();
                            });
                    }
                }
            }
        }

        #endregion

        #region HasPassword

        private bool _hasPassword;

        public bool HasPassword
        {
            get => _hasPassword;
            set => SetProperty(ref _hasPassword, value);
        }

        #endregion

        #region IsInitialPassword

        private bool _isInitialPassword;

        public bool IsInitialPassword
        {
            get => _isInitialPassword;
            set => SetProperty(ref _isInitialPassword, value);
        }

        #endregion

        #region IsPasswordValid

        private bool _isPasswordValid;

        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set => SetProperty(ref _isPasswordValid, value);
        }

        #endregion


        #region IsUserInDatabaseAndHasPassword

        private bool _isUserInDatabaseAndHasPassword;

        public bool IsUserInDatabaseAndHasPassword
        {
            get => _isUserInDatabaseAndHasPassword;
            set => SetProperty(ref _isUserInDatabaseAndHasPassword, value);
        }

        #endregion


        #region Version
        public string Version => Globals.Version;

        #endregion

        #endregion

        #region Ctor

        public MainPageViewModel(IAccountService accountService, INavigationService navigationService, IRepository<User> usersProxy, IPasswordService passwordService, ILogger logger, IRepository<TrainingsTeilnahme> trainingsTeilnahmenRepository, IRepository<Training> trainingRepository, IRepository<Mitglied> mitgliederRepository) : base(navigationService, logger)
        {
            Title = "Anmeldung";

            _accountService = accountService;
            _usersProxy = usersProxy;
            _passwordService = passwordService;
            _trainingsTeilnahmenRepository = trainingsTeilnahmenRepository;
            _trainingRepository = trainingRepository;
            _mitgliederRepository = mitgliederRepository;

            SettingsCommand = new DelegateCommand(OnSettings, CanSettings);
            StartCommand = new DelegateCommand(OnStart, CanStart);
            NewPasswordCommand = new DelegateCommand(OnNewPassword, CanNewPassword);
        }

        #endregion

        #region Commands

        private void OnSettings()
        {
            NavigationService.NavigateAsync("RestApiSettingsPage");
        }

        private static bool CanSettings()
        {
            return true;
        }

        private void OnStart()
        {
            if (!CanStart()) return;

            if (!IsInitialPassword)
            {
                var navigationParametersForPasswordPage = new NavigationParameters {{"User", ServerUser}};

                NavigationService.NavigateAsync("StartPage", navigationParametersForPasswordPage);
            }
            else
            {
                var navigationParametersForPasswordPage = new NavigationParameters { { "User", ServerUser } };

                NavigationService.NavigateAsync("PasswordPage", navigationParametersForPasswordPage);
            }
        }

        private bool CanStart()
        {
            return IsRestApiAccountSet && ServerUser != null && _isPasswordValid;
        }

        private void OnNewPassword()
        {
            
        }

        private bool CanNewPassword()
        {
            return true;
        }

        #endregion

        #region Methods

        private async Task<UserRequest> RequestServerUserAsync(string username, DateTime requestTime)
        {
            //this is all old async code because we user the proxy instead of the cached repositorxy now
            if ((await _usersProxy.GetAsync()).All(user => user.Username != username)) return new UserRequest
            {
                RequestTime = requestTime,
                User = null
            };

            var userRequest = new UserRequest
            {
                RequestTime = requestTime,
                User = (await _usersProxy.GetAsync()).Single(user => user.Username == username)
            };

            return userRequest;
        }

        private void EnablePasswortBoxIfUsernameWasFoundOnServer(User serverUser)
        {
            if (serverUser == null || serverUser.Id == 0)
            {
                IsUsernameInDatabase = false;
                ServerUser = null;
            }
            else
            {
                IsUsernameInDatabase = true;
                ServerUser = serverUser;
            }

            ((DelegateCommand)StartCommand).RaiseCanExecuteChanged();
        }

        private static User GetUserFromTask(Task<UserRequest> getServerUserTask)
        {
            return getServerUserTask.IsFaulted ? null : getServerUserTask.Result.User;
        }

        #endregion

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                if (parameters.InternalParameters.ContainsKey("__NavigationMode") && parameters.InternalParameters["__NavigationMode"].ToString() == "Back")
                {
                    Password = string.Empty;
                    Username = string.Empty;
                }

                //Start caching
                await _mitgliederRepository.GetAsync();
                //await _trainingRepository.GetAsync();
                //await _trainingsTeilnahmenRepository.GetAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }
    }
}
