using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using IT2media.Extensions.Logging.Abstractions;
using Prism.Navigation;
using AVF.MemberManagement.StandardLibrary.Services;

namespace AVF.MemberManagement.ViewModels
{
    public class RestApiSettingsPageViewModel : BindableBase, INavigatingAware
    {
        private readonly ILogger _logger;
        private readonly INavigationService _navigationService;
        private readonly IProxyBase<Setting, string> _settingsProxy; //Leave Proxy instead of Repository, because we use this for connection testing our RestApi, and the Repository may be cached anytime later
        private readonly IRepositoryBootstrapper _repositoryBootstrapper;
        private readonly IJsonFileFactory _jsonFileFactory;

        public IAccountService AccountService { get; }

        private string _message;
        private List<Setting> _settings;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public bool UseFileProxies
        {
            get => Globals.UseFileProxies;
            set 
            {
                Globals.UseFileProxies = value;
                _repositoryBootstrapper.RegisterRepositories(value);
                RaisePropertyChanged("UseFileProxies");
            }
        }

        private string _cacheMessage;

        public string CacheMessage
        {
            get => _cacheMessage;
            set => SetProperty(ref _cacheMessage, value);
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

        public ICommand RefreshCacheCommand { get; }

        public RestApiSettingsPageViewModel(ILogger logger, IAccountService accountService, INavigationService navigationService, IProxyBase<Setting, string> settingsProxy, IRepositoryBootstrapper repositoryBootstrapper, IJsonFileFactory jsonFileFactory)
        {
            _logger = logger;
            _navigationService = navigationService;
            _settingsProxy = settingsProxy;
            _repositoryBootstrapper = repositoryBootstrapper;
            _jsonFileFactory = jsonFileFactory;

            AccountService = accountService;

            SaveCommand = new DelegateCommand(OnSave, CanSave);
            BackCommand = new DelegateCommand(OnBack, CanBack);
            ValidateCommand = new DelegateCommand(OnValidate, CanValidate);
            RefreshCacheCommand = new DelegateCommand(OnRefreshCache, CanRefreshCache);
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

        private bool CanRefreshCache()
        {
            return true;
        }

        private async void OnRefreshCache()
        {
            try
            {
                var factory = _jsonFileFactory;
                var fileList = await factory.RefreshFileCache();

                //Debug.WriteLine("Cached files count:" + fileList.Count);

                CacheMessage = "Cached files count:" + fileList.Count;
            }
            catch (Exception ex)
            {
                CacheMessage = ex.Message;
            }
        }

        #region AdvancedSettingsCommand

        public ICommand AdvancedSettingsCommand { get; }

        private void AdvancedSettings()
        {
        }

        private bool CanAdvancedSettings()
        {
            if (string.IsNullOrEmpty(AccountService.RestApiAccount?.Password)) return false;

            return AccountService.RestApiAccount.Password == _passwordForAdvancedSettings;
        }

        //ctor: 
        //xaml: Command="{Binding AdvancedSettingsCommand}"

        #endregion


        public async void OnNavigatingTo(NavigationParameters parameters)
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
