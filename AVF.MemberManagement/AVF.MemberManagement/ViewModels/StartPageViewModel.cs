using Prism.Mvvm;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class StartPageViewModel : BindableBase, INavigatedAware
    {
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private readonly IUsersProxy _usersProxy;
        private readonly IPasswordService _passwordService;

        public string Title { get; set; }

        public StartPageViewModel(IAccountService accountService, INavigationService navigationService, IUsersProxy usersProxy, IPasswordService passwordService)
        {
            _accountService = accountService;
            _navigationService = navigationService;
            _usersProxy = usersProxy;
            _passwordService = passwordService;
        }

        public async void OnNavigatedFrom(NavigationParameters parameters)
        {
            //Title = (await _settingsProxy.GetSettingAsync("Title")).Value;
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }
    }
}
