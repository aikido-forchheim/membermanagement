using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.Views;

namespace AVF.MemberManagement.ViewModels
{
    public class MainPageViewModel
    {
        public ICommand SettingsCommand { get; }
        public ICommand StartCommand { get; }

        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;

        public bool IsRestApiAccountSet => _accountService.IsRestApiAccountSet;

        public MainPageViewModel(IAccountService accountService, INavigationService navigationService)
        {
            _accountService = accountService;
            _navigationService = navigationService;

            SettingsCommand = new DelegateCommand<object>(OnSettings, CanSettings);
            StartCommand = new DelegateCommand<object>(OnStart, CanStart);
        }

        private void OnSettings(object state)
        {
            _navigationService.NavigateAsync(nameof(RestApiSettingsPage));
        }

        private static bool CanSettings(object state)
        {
            return true;
        }

        private void OnStart(object obj)
        {
            //_navigationService.NavigateAsync(nameof(StartPage));
        }

        private bool CanStart(object arg)
        {
            return IsRestApiAccountSet;
        }
    }

}
