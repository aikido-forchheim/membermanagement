using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Windows.Input;

namespace AVF.CourseParticipation.ViewModels
{
    public class ParticipantsSelectionPageViewModel : MemberSelectionPageViewModel
	{
        private readonly IPageDialogService _dialogService;

        public ParticipantsSelectionPageViewModel(INavigationService navigationService, IRepository<Mitglied> memberRepository, ILogger logger, IPageDialogService dialogService, IRepository<TrainerErnennung> trainerAppointmentsRepository) : base(navigationService, memberRepository, logger, trainerAppointmentsRepository)
        {
            SaveCommand = new DelegateCommand(Save, CanSave);
            _dialogService = dialogService;
        }

        internal async void Cancel()
        {
            var shouldCancel = await _dialogService.DisplayAlertAsync("Abbrechen", "Möchten Sie verlassen OHNE zu speichern?", "Ja", "Nein");

            if (shouldCancel)
            {
                await NavigationService.NavigateAsync("/SaveStatusPage");
            }
        }

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private async void Save()
        {
            await NavigationService.NavigateAsync("SaveParticipantsPage");
        }

        private bool CanSave()
        {
            return true;
        }

        #endregion
    }
}
