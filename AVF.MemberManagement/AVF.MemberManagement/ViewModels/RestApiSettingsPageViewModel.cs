using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.Views;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using IT2media.Extensions.Logging.Abstractions;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class RestApiSettingsPageViewModel : BindableBase, INavigatingAware
    {
        private readonly ILogger _logger;
        public IAccountService AccountService { get; }
        private readonly INavigationService _navigationService;
        private readonly ISettingsProxy _settingsProxy;
        
        private string _message;
        private List<Setting> _settings;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand ValidateCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand BackCommand { get; }

        public RestApiSettingsPageViewModel(ILogger logger, IAccountService accountService, INavigationService navigationService, ISettingsProxy settingsProxy)
        {
            _logger = logger;
            AccountService = accountService;
            _navigationService = navigationService;
            _settingsProxy = settingsProxy;

            SaveCommand = new DelegateCommand(OnSave, CanSave);
            BackCommand = new DelegateCommand(OnBack, CanBack);
            ValidateCommand = new DelegateCommand(OnValidate, CanValidate);
        }

        private bool CanValidate()
        {
            //return !string.IsNullOrWhiteSpace(RestApiAccount?.Username) && !string.IsNullOrWhiteSpace(RestApiAccount.Password) && !string.IsNullOrWhiteSpace(RestApiAccount.ApiUrl);
            return true;
        }

        private async void OnValidate()
        {
            await RunConnectionTest();
        }

        private static bool CanBack()
        {
            return true;
        }

        private void OnBack()
        {
            _navigationService.NavigateAsync(nameof(MainPage));
        }

        private async void OnSave()
        {
            try
            {
                await RunConnectionTest().ContinueWith(task =>
                {

                    if (CanSave())
                    {
                        AccountService.StoreRestApiAccount(AccountService.RestApiAccount.ApiUrl,
                            AccountService.RestApiAccount.Username, AccountService.RestApiAccount.Password);

                        Message = "Account-Informationen erfolgreich gespeichert...";
                    }
                    else
                    {
                        Message = "Account-Informationen temporär verändert, aber nicht gespeichert... Verbindungstest war nicht erfolgreich!";
                    }

                    _logger.LogInformation(Message);
                });
            }
            catch (Exception ex)
            {
                Message = ex.ToString();

                _logger.LogError(ex);
            }
        }

        private bool CanSave()
        {
            var canSave = _settings != null && _settings.Any();

            return canSave;
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            await RunConnectionTest();
        }

        private async Task RunConnectionTest()
        {
            try
            {
                _settings = await _settingsProxy.LoadSettingsCacheAsync(true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                _settings = null;
            }

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }
}
