using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
	public class LoginPageViewModel : ViewModelBase
	{
        public ICommand LoginCommand { get; }

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            LoginCommand = new DelegateCommand(Login, CanLogin);
        }

        private bool CanLogin()
        {
            return true;
        }

        private void Login()
        {
            NavigationService.NavigateAsync("CalenderPage");
        }
    }
}
