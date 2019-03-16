using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Prism.Services;

namespace AVF.CourseParticipation.ViewModels
{
	public class CalenderPageViewModel : ViewModelBase
	{
	    private readonly IPageDialogService _pageDialogService;

	    public ICommand SelectCourseCommand { get; }
	    public ICommand SetTodayCommand { get; }

        private DateTime _selectedDate = DateTime.Today;

	    public DateTime SelectedDate
	    {
	        get => _selectedDate;
	        set => SetProperty(ref _selectedDate, value);
	    }

        public CalenderPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            _pageDialogService = pageDialogService;
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

	    public async void Logout()
	    {
	        if (await _pageDialogService.DisplayAlertAsync("Abmelden?", "Möchten Sie sich wirklich abmelden?", "OK",
	            "Abbrechen"))
	        {
	            //await NavigationService.NavigateAsync("../LoginPage");
	            await NavigationService.GoBackAsync(null, true);
	        }
	    }
	}
}
