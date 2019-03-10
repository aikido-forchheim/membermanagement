using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Views;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace AVF.CourseParticipation.ViewModels
{
	public class TrainingEditPageViewModel : ViewModelBase
	{
	    private DateTime _selectedDate = DateTime.Now;

	    public DateTime SelectedDate
	    {
	        get => _selectedDate;
	        set => SetProperty(ref _selectedDate, value);
	    }

        public TrainingEditPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectParticipantsCommand = new DelegateCommand(SelectParticipants, CanSelectParticipants);
        }

        #region SelectParticipantsCommand

        public ICommand SelectParticipantsCommand { get; }

	    private async void SelectParticipants()
	    {
            await NavigationService.NavigateAsync("app:///NavigationPage/ParticipantsSelectionPage", null, true);
	    }

	    private bool CanSelectParticipants()
	    {
	        return true;
	    }

	    #endregion
	}
}
