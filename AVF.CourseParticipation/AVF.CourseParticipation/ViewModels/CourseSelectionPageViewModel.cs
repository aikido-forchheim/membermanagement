using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AVF.CourseParticipation.Models;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
    public class CourseSelectionPageViewModel : ViewModelBase
    {
        public ObservableCollection<CourseSelectionInfo> CourseSelectionInfos { get; set; }
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

        private DateTime _selectedDate = DateTime.Today;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public CourseSelectionPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}