using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AVF.CourseParticipation.Models;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

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
            var filterTrainingByCourseId = new Filter
            {
                ColumnName = nameof(Training.KursID),
                MatchType = "eq",
                Value = SelectedCourseSelectionInfo.CourseId.ToString()
            };

            var selectedDate = SelectedDate.ToString("s");
            var filterTrainingByDate = new Filter
            {
                ColumnName = nameof(Training.Termin),
                MatchType = "eq",
                Value = selectedDate
            };

            var trainings = await TrainingsRepository.GetAsync(new List<Filter> { filterTrainingByCourseId, filterTrainingByDate });

            Training training;

            if (trainings == null || trainings.Count == 0)
            {
                training = CreateNewTraining(SelectedCourseSelectionInfo.CourseId, SelectedDate);
            }
            else
            {
                training = trainings.Single();

                training.DatensatzGeaendertAm = DateTime.Now;
                training.DatensatzGeaendertVon = (int) LoggedInMemberId;

                //training.Save();
            }

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

            await NavigationService.GoBackAsync();
        }

        private Training CreateNewTraining(int courseId, DateTime selectedDate)
        {
            Training newTraining = new Training();

            newTraining.Id = 0;
            newTraining.Bemerkung = string.Empty;
            newTraining.DatensatzAngelegtAm = DateTime.Now;
            newTraining.DatensatzAngelegtVon = (int) LoggedInMemberId;
            newTraining.Kotrainer1 = null;
            newTraining.Kotrainer2 = null;
            newTraining.Trainer = 0;
            //newTraining.DatensatzGeaendertAm //muss oben in training.Single
            //newTraining.DatensatzGeaendertVon = //muss oben in training.Single
            //newTraining.DauerMinuten = 
            //newTraining.Kindertraining =
            newTraining.KursID = courseId;
            newTraining.Termin = DateTime.Now;
            //newTraining.VHS = false;
            //newTraining.Zeit = 

            return newTraining;
        }

        private bool CanSave()
        {
            return true;
        }

        #endregion
    }
}
