using System;
using System.Diagnostics;
using System.Windows.Input;
using Prism.Mvvm;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Views;
using Prism.Commands;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;
        private readonly IPasswordService _passwordService;

        private string _username;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public StartPageViewModel(IAccountService accountService, INavigationService navigationService, IPasswordService passwordService) : base(navigationService)
        {
            Title = "Startseite";

            _accountService = accountService;
            _passwordService = passwordService;

            NavigateToDaySelectionPageCommand = new DelegateCommand(NavigateToDaySelectionPage, CanNavigateToDaySelectionPage);
        }

        #region NavigateToDaySelectionPageCommand

        public ICommand NavigateToDaySelectionPageCommand { get; }

        private void NavigateToDaySelectionPage()
        {
            _navigationService.NavigateAsync(nameof(DaySelectionPage));
        }

        private bool CanNavigateToDaySelectionPage()
        {
            return true;
        }

        #endregion

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                var user = (User)parameters["User"];
                if (user == null) return;

                Globals.User = user;
                Username = user.Username;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
