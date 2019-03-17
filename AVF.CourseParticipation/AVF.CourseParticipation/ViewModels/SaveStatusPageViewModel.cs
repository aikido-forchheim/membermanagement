using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AVF.CourseParticipation.ViewModels
{
	public class SaveStatusPageViewModel : ViewModelBase
	{
        private readonly IPageDialogService _dialogService;

        public SaveStatusPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            BackCommand = new DelegateCommand(GoBack, CanGoBack);
            _dialogService = dialogService;
        }

        #region SaveCommand

        public ICommand BackCommand { get; }

        internal async void GoBack()
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
