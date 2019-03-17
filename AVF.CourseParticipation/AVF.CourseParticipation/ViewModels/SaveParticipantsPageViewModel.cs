using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;

namespace AVF.CourseParticipation.ViewModels
{
    public class SaveParticipantsPageViewModel : ViewModelBase
	{
        private readonly IPageDialogService _dialogService;

        public SaveParticipantsPageViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            SaveCommand = new DelegateCommand(Save, CanSave);
            CancelCommand = new DelegateCommand(Cancel, CanCancel);
            _dialogService = dialogService;
        }

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private async void Save()
        {
            //Do the save stuff, and if an error occurs do pass error status to SaveStatusPage
            //Schreibe auf alle Fälle alle Namen und das Datum ins Log

            await NavigationService.NavigateAsync("/SaveStatusPage");
        }

        private bool CanSave()
        {
            return true;
        }

        #endregion

        #region CancelCommand

        public ICommand CancelCommand { get; }

        private async void Cancel()
        {
            //Prompt for dialog and pass canceled status to SaveStatusPage
            //Schreibe auch hier tortzdem auf alle Fälle alle Namen und das Datum ins Log mit Status cancel

            var shouldCancel = await _dialogService.DisplayAlertAsync("Abbrechen", "Möchten Sie verlassen OHNE zu speichern?", "Ja", "Nein");

            if (shouldCancel)
            {
                await NavigationService.NavigateAsync("/SaveStatusPage");
            }
        }

        private bool CanCancel()
        {
            return true;
        }

        #endregion
    }
}
