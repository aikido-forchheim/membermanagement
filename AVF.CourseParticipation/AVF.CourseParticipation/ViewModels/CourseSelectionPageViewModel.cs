using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Models;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.StandardLibrary.Extensions;
using Microsoft.Extensions.Logging;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
    public class CourseSelectionPageViewModel : ViewModelBase
    {
        private readonly ILogger _logger;
        private readonly IRepository<Kurs> _courseRepository;
        private readonly IRepository<Mitglied> _memberRepository;
        public ObservableCollection<CourseSelectionInfo> CourseSelectionInfos { get; set; } = new ObservableCollection<CourseSelectionInfo>();

        private DateTime _selectedDate = DateTime.Today;

        private CourseSelectionInfo _selectedCourseSelectionInfo;

        public CourseSelectionInfo SelectedCourseSelectionInfo
        {
            get => _selectedCourseSelectionInfo;
            set => SetProperty(ref _selectedCourseSelectionInfo, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public CourseSelectionPageViewModel(INavigationService navigationService, ILogger logger, IRepository<Kurs> courseRepository, IRepository<Mitglied> memberRepository) : base(navigationService)
        {
            _logger = logger;
            _courseRepository = courseRepository;
            _memberRepository = memberRepository;

            EnterParticipantsCommand = new DelegateCommand(EnterParticipants, CanEnterParticipants).ObservesProperty(() => SelectedCourseSelectionInfo);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("SelectedDate"))
            {
                try
                {
                    var selectedDate = parameters["SelectedDate"].ToString();
                    DateTime.TryParse(selectedDate, out var parsedDate);
                    SelectedDate = parsedDate;

                    var courses = await _courseRepository.GetAsync();
                    var relevantCourses = courses.Where(c =>
                    {
                        var selectedDateDayOfWeek = (int) SelectedDate.DayOfWeek;
                        if (selectedDateDayOfWeek == 0) selectedDateDayOfWeek = 7;
                        return c.WochentagID == selectedDateDayOfWeek;
                    }).OrderBy(c => c.Zeit);

                    CourseSelectionInfos.Clear();

                    foreach (var course in relevantCourses)
                    {
                        var courseSelectionInfo = new CourseSelectionInfo
                        {
                            MemberId = course.Trainer,
                            From = course.Zeit,
                            CourseId = course.Id
                        };

                        courseSelectionInfo.To = courseSelectionInfo.From + new TimeSpan(0, 0, course.DauerMinuten, 0);

                        if (course.Kotrainer1 != null && course.Kotrainer1 != -1) courseSelectionInfo.ContrainerMemberIds.Add(course.Kotrainer1);
                        if (course.Kotrainer2 != null && course.Kotrainer2 != -1) courseSelectionInfo.ContrainerMemberIds.Add(course.Kotrainer2);

                        var member = await _memberRepository.GetAsync(courseSelectionInfo.MemberId);

                        courseSelectionInfo.LastName = member.Nachname;
                        courseSelectionInfo.FirstName = member.FirstName;

                        CourseSelectionInfos.Add(courseSelectionInfo);
                    }

                    if (SelectedDate.IsToday())
                    {
                        for (var i = CourseSelectionInfos.Count - 1; i >= 0; i--)
                        {
                            var courseSelectionInfo = CourseSelectionInfos[i];

                            var now = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                            if (courseSelectionInfo.From < now)
                            {
                                SelectedCourseSelectionInfo = courseSelectionInfo;
                                return;
                            }
                        }

                        SelectedCourseSelectionInfo = CourseSelectionInfos.First();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }
            }
        }

        #region EnterPartipantsCommand

        public ICommand EnterParticipantsCommand { get; }

        private void EnterParticipants()
        {
            try
            {
                var parameters = new NavigationParameters
                {
                    {nameof(SelectedDate), SelectedDate},
                    {nameof(SelectedCourseSelectionInfo), SelectedCourseSelectionInfo.Serialize()}
                };
                NavigationService.NavigateAsync("TrainingEditPage", parameters);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
            }
        }

        private bool CanEnterParticipants()
        {
            return SelectedCourseSelectionInfo != null;
        }

        #endregion  
    }
}