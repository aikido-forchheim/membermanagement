using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AVF.CourseParticipation.ViewModels
{
	public class TrainingEditPageViewModel : BindableBase
	{
	    private DateTime _selectedDate = DateTime.Now;

	    public DateTime SelectedDate
	    {
	        get => _selectedDate;
	        set => SetProperty(ref _selectedDate, value);
	    }

        public TrainingEditPageViewModel()
        {

        }
	}
}
