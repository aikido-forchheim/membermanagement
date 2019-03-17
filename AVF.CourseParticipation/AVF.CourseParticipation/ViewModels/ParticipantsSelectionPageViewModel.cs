using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AVF.CourseParticipation.ViewModels
{
	public class ParticipantsSelectionPageViewModel : ViewModelBase
	{
        public ParticipantsSelectionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SaveCommand = new DelegateCommand(Save, CanSave);
        }

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private async void Save()
        {
            //await NavigationService.NavigateAsync("CalendarPage", null, true);
            await NavigationService.NavigateAsync("/NavigationPage/CalenderPage");
        }

        private bool CanSave()
        {
            return true;
        }

        #endregion
    }
}
