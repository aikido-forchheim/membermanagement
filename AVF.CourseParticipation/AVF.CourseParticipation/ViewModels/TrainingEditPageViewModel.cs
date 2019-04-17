using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Models;
using AVF.CourseParticipation.ViewModels.Models;
using AVF.CourseParticipation.Views;
using AVF.StandardLibrary.Extensions;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace AVF.CourseParticipation.ViewModels
{
	public class TrainingEditPageViewModel : ViewModelBase
	{
	    private readonly ILogger _logger;
	    private DateTime _selectedDate = DateTime.Now;

	    public DateTime SelectedDate
	    {
	        get => _selectedDate;
	        set => SetProperty(ref _selectedDate, value);
	    }

	    private ObservableCollection<TrainerInfo> _trainerInfos = new ObservableCollection<TrainerInfo>();

	    public ObservableCollection<TrainerInfo> TrainerInfos
	    {
	        get => _trainerInfos;
	        set => SetProperty(ref _trainerInfos, value);
	    }

	    private CourseSelectionInfo _selectedCourseSelectionInfo;

	    public CourseSelectionInfo SelectedCourseSelectionInfo
        {
	        get => _selectedCourseSelectionInfo;
	        set => SetProperty(ref _selectedCourseSelectionInfo, value);
	    }

        public TrainingEditPageViewModel(INavigationService navigationService, ILogger logger) : base(navigationService)
        {
            _logger = logger;
            SelectParticipantsCommand = new DelegateCommand(SelectParticipants, CanSelectParticipants);
        }

	    public override async void OnNavigatedTo(INavigationParameters parameters)
	    {
	        try
	        {
	            base.OnNavigatedTo(parameters);

	            if (parameters.ContainsKey(nameof(SelectedDate)))
	            {
	                if (DateTime.TryParse(parameters[nameof(SelectedDate)].ToString(), out var parsedDate))
	                {
	                    SelectedDate = parsedDate;
	                }
	            }

	            if (parameters.ContainsKey(nameof(SelectedCourseSelectionInfo)))
	            {
	                TrainerInfos.Clear();

	                SelectedCourseSelectionInfo =
	                    parameters[nameof(SelectedCourseSelectionInfo)].ToString().Deserialize<CourseSelectionInfo>();

                    TrainerInfos.Add(new TrainerInfo { FullName = SelectedCourseSelectionInfo.FirstName + " " + SelectedCourseSelectionInfo.LastName });
	                //TrainerInfos.Add(new TrainerInfo {FullName = "Markus Malz"});
	            }
            }
	        catch (Exception e)
	        {
	            _logger.LogError(e.ToString());
	        }
	    }

        #region SelectParticipantsCommand

        public ICommand SelectParticipantsCommand { get; }

	    private async void SelectParticipants()
	    {
            await NavigationService.NavigateAsync("/NavigationPage/ParticipantsSelectionPage");
	    }

	    private bool CanSelectParticipants()
	    {
	        return true;
	    }

	    #endregion
	}
}
