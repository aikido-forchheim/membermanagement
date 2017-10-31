using Prism.Mvvm;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class StartPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IPasswordService _passwordService;

        public string Title { get; set; }

        private string _username;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public StartPageViewModel(IAccountService accountService, INavigationService navigationService, IPasswordService passwordService)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            _passwordService = passwordService;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            //Title = (await _settingsProxy.GetSettingAsync("Title")).Value;
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            var user = (User) parameters["User"];

            Globals.User = user;

            Username = user.Username;
        }
    }
}
