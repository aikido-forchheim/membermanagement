using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Models;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
    public class CourseSelectionPageViewModel : ViewModelBase
    {
        public ObservableCollection<CourseSelectionInfo> CourseSelectionInfos { get; set; }

        private DateTime _selectedDate = DateTime.Today;

        private CourseSelectionInfo _courseSelectionInfo;

        public CourseSelectionInfo CourseSelectionInfo
        {
            get => _courseSelectionInfo;
            set => SetProperty(ref _courseSelectionInfo, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public CourseSelectionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            EnterParticipantsCommand = new DelegateCommand(EnterParticipants, CanEnterParticipants);

            CourseSelectionInfos

            = new ObservableCollection<CourseSelectionInfo>
            {
                new CourseSelectionInfo
                {
                    From = new TimeSpan(0, 10, 15, 0),
                    To = new TimeSpan(0, 11, 30, 0),
                    FirstName = "Hermann",
                    LastName = "Kirsch",
                    Participants = 9
                },
                new CourseSelectionInfo
                {
                    From = new TimeSpan(0, 17, 0, 0),
                    To = new TimeSpan(0, 19, 0, 0),
                    FirstName = "Günther",
                    LastName = "Reich",
                    Participants = 12
                },
                new CourseSelectionInfo
                {
                    From = new TimeSpan(0, 19, 0, 0),
                    To = new TimeSpan(0, 21, 0, 0),
                    FirstName = "Klaus",
                    LastName = "Schmoranz",
                    Participants = 7
                },
            };
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            CourseSelectionInfo = CourseSelectionInfos.Last();
        }

        #region EnterPartipantsCommand

        public ICommand EnterParticipantsCommand { get; }

        private void EnterParticipants()
        {
        }

        private bool CanEnterParticipants()
        {
            return true;
        }

        #endregion  
    }
}