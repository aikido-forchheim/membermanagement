using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using IT2media.Extensions.Logging.Abstractions;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class RestApiSettingsPageViewModel : ViewModelBase
    {
        private readonly ILogger _logger;
        private readonly IProxyBase<Setting, string> _settingsProxy; //Leave Proxy instead of Repository, because we use this for connection testing our RestApi, and the Repository may be cached anytime later

        public IAccountService AccountService { get; }

        private string _message;
        private List<Setting> _settings;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private string _passwordForAdvancedSettings;

        public string PasswordForAdvancedSettings
        {
            get => _passwordForAdvancedSettings;
            set
            {
                SetProperty(ref _passwordForAdvancedSettings, value); 
                
                ((DelegateCommand) AdvancedSettingsCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand ValidateCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand BackCommand { get; }

        public RestApiSettingsPageViewModel(ILogger logger, IAccountService accountService, INavigationService navigationService, IProxyBase<Setting, string> settingsProxy) : base(navigationService)
        {
            Title = "Einstellungen";

            _logger = logger;
            _settingsProxy = settingsProxy;

            AccountService = accountService;

            SaveCommand = new DelegateCommand(OnSave, CanSave);
            BackCommand = new DelegateCommand(OnBack, CanBack);
            ValidateCommand = new DelegateCommand(OnValidate, CanValidate);
            AdvancedSettingsCommand = new DelegateCommand(AdvancedSettings, CanAdvancedSettings);
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
            _navigationService.NavigateAsync("MainPage");
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

        #region AdvancedSettingsCommand

        public ICommand AdvancedSettingsCommand { get; }

        private void AdvancedSettings()
        {
            _navigationService.NavigateAsync("AdvancedSettingsPage");
        }

        private bool CanAdvancedSettings()
        {
            if (string.IsNullOrEmpty(AccountService.RestApiAccount?.Password)) return false;

            return AccountService.RestApiAccount.Password == _passwordForAdvancedSettings;
        }

        #endregion


        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            await RunConnectionTest();
        }

        private async Task RunConnectionTest()
        {
            try
            {
                _settings = await _settingsProxy.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex);
                _settings = null;
            }

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }
}
