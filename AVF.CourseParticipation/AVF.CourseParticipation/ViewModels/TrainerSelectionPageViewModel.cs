using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AVF.CourseParticipation.Models;
using AVF.MemberManagement.StandardLibrary.Interfaces;
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
            await NavigationService.GoBackAsync();
        }

        private bool CanSave()
        {
            return true;
        }

        #endregion
    }
}
