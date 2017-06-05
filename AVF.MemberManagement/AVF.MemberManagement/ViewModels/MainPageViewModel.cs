using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.MemberManagement.StandardLibrary.Interfaces;

namespace AVF.MemberManagement.ViewModels
{
    public class MainPageViewModel
    {
        public ICommand SettingsCommand { get; private set; }
        public ICommand StartCommand { get; private set; }

        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;

        public bool IsRestApiAccountSet
        {
            get
            {
                return _accountService.IsRestApiAccountSet;
            }
            set
            {
            }
        }

        public MainPageViewModel(IAccountService accountService, INavigationService navigationService)
        {
            _accountService = accountService;
            _navigationService = navigationService;

            SettingsCommand = new DelegateCommand<object>(this.OnSettings, this.CanSettings);
            StartCommand = new DelegateCommand<object>(this.OnStart, this.CanStart);
        }

        private void OnSettings(object state)
        {
            //_navigationService.NavigateAsync(nameof(RestApiSettingsPage));
        }
        private bool CanSettings(object state)
        {
            return true;
        }

        void OnStart(object obj)
        {
            //_navigationService.NavigateAsync(nameof(StartPage));
        }

        bool CanStart(object arg)
        {
            return IsRestApiAccountSet;
        }
    }
}
