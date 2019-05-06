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

        public ParticipantsSelectionPageViewModel(INavigationService navigationService, IRepository<Mitglied> memberRepository, ILogger logger, IPageDialogService dialogService, IRepository<TrainerErnennung> trainerAppointmentsRepository, IRepository<Training> trainingsRepository, IRepository<TrainingsTeilnahme> trainingParticipationsRepository) 
            : base(navigationService, memberRepository, logger, trainerAppointmentsRepository, trainingsRepository, trainingParticipationsRepository, dialogService)
        {
	        CancelCommand = new DelegateCommand(Cancel, CanCancel);
            SaveCommand = new DelegateCommand(Save, CanSave);
            _dialogService = dialogService;
	    }

	    private bool CanCancel()
	    {
	        return true;
	    }

	    public ICommand CancelCommand { get; }

        internal async void Cancel()
        {
            var shouldCancel = await _dialogService.DisplayAlertAsync("Abbrechen", "Möchten Sie verlassen OHNE zu speichern?", "Ja", "Nein");

            if (shouldCancel)
            {
                await NavigationService.NavigateAsync("/SaveStatusPage");
            }
        }

        #region SaveCommand

        protected override async void Save()
        {
            base.Save();

            await NavigationService.NavigateAsync("SaveParticipantsPage");
        }

	    protected override bool CanSave()
	    {
	        var baseCanSave = base.CanSave();

            return baseCanSave;
        }

        #endregion
    }
}
