﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Models;
using AVF.CourseParticipation.Views;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.StandardLibrary.Extensions;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace AVF.CourseParticipation.ViewModels
{
	public class TrainingEditPageViewModel : ViewModelBase
	{
	    private readonly ILogger _logger;
	    private readonly IRepository<Mitglied> _memberRepository;
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

        public ICommand EditTrainerCommand { get; }

        public TrainingEditPageViewModel(INavigationService navigationService, ILogger logger, IRepository<Mitglied> memberRepository) : base(navigationService)
        {
            _logger = logger;
            _memberRepository = memberRepository;
            SelectParticipantsCommand = new DelegateCommand(SelectParticipants, CanSelectParticipants);
            EditTrainerCommand = new DelegateCommand(EditTrainer, CanEditTrainer);
        }

	    private bool CanEditTrainer()
	    {
	        return true;
	    }

	    private async void EditTrainer()
	    {
	        var parameters = new NavigationParameters
	        {
	            {nameof(SelectedDate), SelectedDate},
	            { nameof(SelectedCourseSelectionInfo), SelectedCourseSelectionInfo.Serialize() }
	        };

	        await NavigationService.NavigateAsync("TrainerSelectionPage", parameters);
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

                    TrainerInfos.Add(new TrainerInfo { FirstName = SelectedCourseSelectionInfo.FirstName, LastName = SelectedCourseSelectionInfo.LastName });

	                foreach (var contrainerMemberId in SelectedCourseSelectionInfo.ContrainerMemberIds)
	                {
	                    if (contrainerMemberId != null && contrainerMemberId != -1)
	                    {
	                        var member = await _memberRepository.GetAsync((int) contrainerMemberId);

	                        TrainerInfos.Add(new TrainerInfo { FirstName = member.FirstName, LastName = member.Nachname});
                        }
	                }
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
	        var parameters = new NavigationParameters
	        {
	            {nameof(SelectedCourseSelectionInfo), SelectedCourseSelectionInfo.Serialize()}
	        };
	        await NavigationService.NavigateAsync("/NavigationPage/ParticipantsSelectionPage",parameters);
	    }

	    private bool CanSelectParticipants()
	    {
	        return true;
	    }

	    #endregion
	}
}
