using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class RestApiSettingsPageViewModel : BindableBase
    {
        private readonly ILogger _logger;
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;

        public RestApiAccount RestApiAccount => _accountService.RestApiAccount;

        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand SaveCommand { get; }

        public ICommand BackCommand { get; }

        public RestApiSettingsPageViewModel(ILogger logger, IAccountService accountService, INavigationService navigationService)
        {
            _logger = logger;
            _accountService = accountService;
            _navigationService = navigationService;

            SaveCommand = new DelegateCommand<object>(OnSave, CanSave);
            BackCommand = new DelegateCommand(OnBack, CanBack);
        }

        private static bool CanBack()
        {
            return true;
        }

        private void OnBack()
        {
            _navigationService.NavigateAsync(nameof(MainPage));
        }

        private void OnSave(object state)
        {
            try
            {
                _accountService.StoreRestApiAccount(_accountService.RestApiAccount.ApiUrl, _accountService.RestApiAccount.Username, _accountService.RestApiAccount.Password);

                Message = "Account-Informationen erfolgreich gespeichert...";

                _logger.LogInformation(Message);
            }
            catch (Exception ex)
            {
                Message = ex.ToString();

                _logger.LogError(ex);
            }
        }

        private static bool CanSave(object state)
        {
            return true;
        }
    }
}
