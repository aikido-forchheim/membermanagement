using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Logging;

namespace AVF.CourseParticipation.ViewModels
{
	public class SettingsPageViewModel : ViewModelBase
	{
	    private readonly ILogger _logger;
	    private readonly IAccountService _accountService;
	    private readonly IProxyBase<Setting, string> _settingsProxy;
	    private readonly RestApiAccount _restApiAccount;

        private List<Setting> _settings;

        private string _message;

	    public string Message
	    {
	        get => _message;
	        set => SetProperty(ref _message, value);
	    }

	    private string _apiUrl;

	    public string ApiUrl
	    {
	        get => _apiUrl;
	        set => SetProperty(ref _apiUrl, value);
	    }

	    private string _username;

	    public string Username
	    {
	        get => _username;
	        set => SetProperty(ref _username, value);
	    }

	    private string _password;

	    public string Password
	    {
	        get => _password;
	        set => SetProperty(ref _password, value);
	    }

	    public ICommand TestCommand { get; }
        public ICommand SaveCommand { get; }

        public SettingsPageViewModel(ILogger logger, INavigationService navigationService, IAccountService accountService, IProxyBase<Setting, string> settingsProxy)
         : base(navigationService)
        {
            _logger = logger;
            _accountService = accountService;
            _settingsProxy = settingsProxy;

            _restApiAccount = accountService.RestApiAccount;

            ApiUrl = _restApiAccount?.ApiUrl;
            Username = _restApiAccount?.Username;
            Password = _restApiAccount?.Password;

            TestCommand = new DelegateCommand(Test, CanTest);
            SaveCommand = new DelegateCommand(Save, CanSave);
        }

	    private bool CanSave()
	    {
	        var canSave = _settings != null && _settings.Any();
	        return canSave;
	    }

        private async void Save()
	    {
	        try
	        {
	            await RunConnectionTest().ContinueWith(task =>
	            {

	                if (CanSave())
	                {
	                    _accountService.StoreRestApiAccount(_accountService.RestApiAccount.ApiUrl,
	                        _accountService.RestApiAccount.Username, _accountService.RestApiAccount.Password);

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

	            _logger.LogError(ex.ToString());
	        }
	    }

        private bool CanTest()
        {
            return true;
        }

	    private async void Test()
	    {
	        try
	        {
	            _restApiAccount.ApiUrl = _apiUrl;
	            _restApiAccount.Username = _username;
	            _restApiAccount.Password = _password;

	            await RunConnectionTest().ContinueWith(task =>
	            {
	                if (CanSave())
	                {
	                    _accountService.StoreRestApiAccount(_accountService.RestApiAccount.ApiUrl,
	                        _accountService.RestApiAccount.Username, _accountService.RestApiAccount.Password);

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

	            _logger.LogError(ex.ToString());
	        }
        }

	    private async Task RunConnectionTest()
	    {
	        try
	        {
	            _settings = await _settingsProxy.GetAsync();
	        }
	        catch (Exception ex)
	        {
	            _logger.LogInformation(ex.ToString());
	            _settings = null;
	        }

	        ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
	    }

	    public override void OnNavigatedTo(INavigationParameters parameters)
	    {
	        base.OnNavigatedTo(parameters);
	    }
    }
}
