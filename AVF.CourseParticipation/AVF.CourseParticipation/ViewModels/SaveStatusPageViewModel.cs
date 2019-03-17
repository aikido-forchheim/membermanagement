using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AVF.CourseParticipation.ViewModels
{
	public class SaveStatusPageViewModel : ViewModelBase
	{
        public SaveStatusPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            BackCommand = new DelegateCommand(GoBack, CanGoBack);
        }

        #region SaveCommand

        public ICommand BackCommand { get; }

        private async void GoBack()
        {
            await NavigationService.NavigateAsync("/NavigationPage/CalenderPage");
        }

        private bool CanGoBack()
        {
            return true;
        }

        #endregion
    }
}
