using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
	public class CalenderPageViewModel : ViewModelBase
	{
	    public ICommand SelectCourseCommand { get; }
	    public ICommand SetTodayCommand { get; }

        private DateTime _selectedDate = DateTime.Today;

	    public DateTime SelectedDate
	    {
	        get => _selectedDate;
	        set => SetProperty(ref _selectedDate, value);
	    }

        public CalenderPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectCourseCommand = new DelegateCommand(SelectCourse, CanSelectCourse);
            SetTodayCommand = new DelegateCommand(SetToday, CanSetToday);
        }

        private bool CanSetToday()
        {
            return true;
        }

        private void SetToday()
        {
            SelectedDate = DateTime.Today;
        }

        private bool CanSelectCourse()
        {
            return true;
        }

        private void SelectCourse()
        {
            NavigationService.NavigateAsync("CourseSelectionPage");
        }
    }
}
