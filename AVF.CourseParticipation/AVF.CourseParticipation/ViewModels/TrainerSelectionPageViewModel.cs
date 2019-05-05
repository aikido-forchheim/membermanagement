using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.StandardLibrary.Extensions;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;
using System.Windows.Input;

namespace AVF.CourseParticipation.ViewModels
{
    public class TrainerSelectionPageViewModel : MemberSelectionPageViewModel
    {
        private readonly IPageDialogService _dialogService;

        public TrainerSelectionPageViewModel(INavigationService navigationService, IRepository<Mitglied> memberRepository, ILogger logger, IRepository<TrainerErnennung> trainerAppointmentsRepository, IRepository<Training> trainingsRepository, IRepository<TrainingsTeilnahme> trainingParticipationsRepository, IPageDialogService dialogService) 
            : base(navigationService, memberRepository, logger, trainerAppointmentsRepository, trainingsRepository, trainingParticipationsRepository, dialogService)
        {
            SaveCommand = new DelegateCommand(Save, CanSave);
            CancelCommand = new DelegateCommand(Cancel, CanCancel);
            _dialogService = dialogService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await OnNavigatedToAsync(parameters);

            AddSelectedMember(SelectedCourseSelectionInfo.MemberId);

            foreach (var contrainerMemberId in SelectedCourseSelectionInfo.ContrainerMemberIds)
            {
                if (contrainerMemberId == null || !(contrainerMemberId > 0)) continue;

                var memberId = (int)contrainerMemberId;
                AddSelectedMember(memberId);
            }
        }

        private void AddSelectedMember(int memberId)
        {
            var member = Members.Single(m => m.MemberId == memberId);
            AddSelectedMember(member);
        }

        #region CancelCommand

        public ICommand CancelCommand { get; }

        internal async void Cancel()
        {
            var shouldCancel = await _dialogService.DisplayAlertAsync("Abbrechen", "Möchten Sie verlassen OHNE zu speichern?", "Ja", "Nein");

            if (shouldCancel)
            {
                await NavigationService.GoBackAsync();
            }
        }

        private bool CanCancel()
        {
            return true;
        }

        #endregion

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private async void Save()
        {
            var trainingsRepository = TrainingsRepository;
            var selectedDate = SelectedDate;
            var courseId = SelectedCourseSelectionInfo.CourseId;

            var trainings = await trainingsRepository.GetAsync(courseId, selectedDate);

            Training training;

            if (trainings == null || trainings.Count == 0)
            {
                training = CreateNewTraining(SelectedCourseSelectionInfo.CourseId, SelectedDate);
            }
            else
            {
                training = trainings.Single();
            }

            //TODO: only update if really something changes
            training.DatensatzGeaendertAm = DateTime.Now;
            training.DatensatzGeaendertVon = (int)LoggedInMemberId;

            //Reset always and than apply new selected values
            training.Trainer = -1;
            training.Kotrainer1 = -1;
            training.Kotrainer2 = -1;

            var i = 0;
            foreach (var selectedMember in SelectedMembers)
            {
                switch (i)
                {
                    case 0:
                        training.Trainer = selectedMember.MemberId;
                        break;
                    case 1:
                        training.Kotrainer1 = selectedMember.MemberId;
                        break;
                    case 2:
                        training.Kotrainer2 = selectedMember.MemberId;
                        break;
                }

                i++;
            }

            if (training.Id == 0)
            {
                training.Id = await TrainingsRepository.CreateAsync(training);
            }
            else
            {
                await TrainingsRepository.UpdateAsync(training);
            }

            var parameters
                = new NavigationParameters { { nameof(Training), training } };
            await NavigationService.GoBackAsync(parameters);
        }

        private Training CreateNewTraining(int courseId, DateTime selectedDate)
        {
            Training newTraining = new Training();

            newTraining.Id = 0; //0 is the identifier for the proxy for auto generating an id, because -1 is an allowed value (for example if we want to use the negative ids later for some purposes)
            newTraining.Bemerkung = string.Empty;
            newTraining.DatensatzAngelegtAm = DateTime.Now;
            newTraining.DatensatzAngelegtVon = (int) LoggedInMemberId;
            newTraining.Kotrainer1 = -1; //erledigt, siehe oben
            newTraining.Kotrainer2 = -1; //erledigt, siehe oben
            newTraining.Trainer = -1; //erledigt, siehe oben
            //newTraining.DatensatzGeaendertAm //erledigt, siehe oben in training.Single
            //newTraining.DatensatzGeaendertVon = //erledigt, siehe oben in training.Single
            newTraining.DauerMinuten = Convert.ToInt32(SelectedCourseSelectionInfo.Duration.TotalMinutes);
            newTraining.Kindertraining = SelectedCourseSelectionInfo.ChildrensTraining;
            newTraining.KursID = courseId;
            newTraining.Termin = SelectedDate;
            //newTraining.VHS = false; //ignore until later update
            newTraining.Zeit = SelectedCourseSelectionInfo.From;
            newTraining.WochentagID = SelectedCourseSelectionInfo.DayOfWeekId;

            return newTraining;
        }

        private bool CanSave()
        {
            return true;
        }

        #endregion
    }
}
