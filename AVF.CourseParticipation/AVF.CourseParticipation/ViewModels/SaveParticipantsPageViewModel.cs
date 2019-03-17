using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace AVF.CourseParticipation.ViewModels
{
    public class SaveParticipantsPageViewModel : ViewModelBase
	{
        public SaveParticipantsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SaveCommand = new DelegateCommand(Save, CanSave);
        }

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private async void Save()
        {
            //Do the save stuff, and if an error occurs do not navigate to NavigationPage/CalenderPage but to an errorpage instead
            //Schreibe auf alle Fälle alle Namen und das Datum ins Log

            await NavigationService.NavigateAsync("/NavigationPage/CalenderPage");
        }

        private bool CanSave()
        {
            return true;
        }

        #endregion
    }
}
