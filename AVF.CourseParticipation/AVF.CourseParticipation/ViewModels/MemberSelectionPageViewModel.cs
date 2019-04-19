using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AVF.CourseParticipation.Extensions;
using AVF.CourseParticipation.Models;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.StandardLibrary.Extensions;
using Microsoft.Extensions.Logging;

namespace AVF.CourseParticipation.ViewModels
{
	public class MemberSelectionPageViewModel : ViewModelBase
	{
	    private readonly IRepository<Mitglied> _memberRepository;
	    private readonly ILogger _logger;
	    private readonly IRepository<TrainerErnennung> _trainerAppointmentsRepository;
	    private readonly IRepository<Training> _trainingsRepository;

	    private static List<Mitglied> _allMembers = new List<Mitglied>();
	    private static List<TrainerErnennung> _trainerAppointments = new List<TrainerErnennung>();

        private ObservableCollection<MemberInfo> _members = new ObservableCollection<MemberInfo>();

	    public ObservableCollection<MemberInfo> Members
	    {
	        get => _members;
	        set => SetProperty(ref _members, value);
	    }

	    private MemberInfo _selectedMember;

	    public MemberInfo SelectedMember
	    {
	        get => _selectedMember;
	        set => SetProperty(ref _selectedMember, value);
	    }

	    private ObservableCollection<MemberInfo> _selectedMembers = new ObservableCollection<MemberInfo>();

	    public ObservableCollection<MemberInfo> SelectedMembers
	    {
	        get => _selectedMembers;
	        set => SetProperty(ref _selectedMembers, value);
	    }

	    private MemberInfo _selectedMemberToRemove;

	    public MemberInfo SelectedMemberToRemove
	    {
	        get => _selectedMemberToRemove;
	        set => SetProperty(ref _selectedMemberToRemove, value);
	    }

	    private bool _onlyActiveMembers = true;

	    public bool OnlyActiveMembers
	    {
	        get => _onlyActiveMembers;
	        set
	        {
	            SetProperty(ref _onlyActiveMembers, value);
	            Filter().IgnoreResult();
	        }
	    }

	    private bool _onlyTrainers;

	    public bool OnlyTrainers
	    {
	        get => _onlyTrainers;
	        set
	        {
	            SetProperty(ref _onlyTrainers, value);
	            Filter().IgnoreResult();
            }
	    }

	    private bool _onlyLastAttendees = true;
	    public bool OnlyLastAttendees
        {
	        get => _onlyLastAttendees;
	        set
	        {
	            SetProperty(ref _onlyLastAttendees, value);
	            Filter().IgnoreResult();
	        }
	    }

	    private int _onlyLastAttendeesMonths = 3;
	    public int OnlyLastAttendeesMonths
        {
	        get => _onlyLastAttendeesMonths;
	        set
	        {
	            SetProperty(ref _onlyLastAttendeesMonths, value);
	            Filter().IgnoreResult();
	        }
	    }

        private bool _withChildren = true;

	    public bool WithChildren
	    {
	        get => _withChildren;
	        set
	        {
	            SetProperty(ref _withChildren, value);
	            Filter().IgnoreResult();
	        }
	    }

	    private bool _withTeenagers = true;

	    public bool WithTeenagers
        {
	        get => _withTeenagers;
	        set
	        {
	            SetProperty(ref _withTeenagers, value);
	            Filter().IgnoreResult();
	        }
	    }

	    private bool _withAdults = true;

	    public bool WithAdults
	    {
	        get => _withAdults;
	        set
	        {
	            SetProperty(ref _withAdults, value);
	            Filter().IgnoreResult();
	        }
	    }

	    private CourseSelectionInfo _selectedCourseSelectionInfo;

	    public CourseSelectionInfo SelectedCourseSelectionInfo
	    {
	        get => _selectedCourseSelectionInfo;
	        set => SetProperty(ref _selectedCourseSelectionInfo, value);
	    }

        public ICommand AddSelectedMemberCommand { get; }
	    public ICommand RemoveSelectedMemberCommand { get; }

        public MemberSelectionPageViewModel(INavigationService navigationService, IRepository<Mitglied> memberRepository, ILogger logger, IRepository<TrainerErnennung> trainerAppointmentsRepository, IRepository<Training> trainingsRepository) : base(navigationService)
	    {
	        _memberRepository = memberRepository;
	        _logger = logger;
	        _trainerAppointmentsRepository = trainerAppointmentsRepository;
	        _trainingsRepository = trainingsRepository;

	        AddSelectedMemberCommand = new DelegateCommand(AddSelectedMember, CanAddSelectedMember).ObservesProperty(() => SelectedMember);
	        RemoveSelectedMemberCommand = new DelegateCommand(RemoveSelectedMember, CanRemoveSelectedMember).ObservesProperty(() => SelectedMemberToRemove);
        }

        private bool CanAddSelectedMember()
        {
            return SelectedMember != null;
        }

	    private void AddSelectedMember()
	    {
	        if (SelectedMember != null)
	        {
	            if (!SelectedMembers.Contains(SelectedMember))
	            {
	                SelectedMembers.Add(SelectedMember);
	            }
	        }

	        if (Members.Contains(SelectedMember))
	        {
	            Members.Remove(SelectedMember);
	        }
        }

	    private bool CanRemoveSelectedMember()
	    {
	        return SelectedMemberToRemove != null;
	    }

	    private void RemoveSelectedMember()
	    {
	        if (SelectedMemberToRemove != null)
	        {
	            if (!Members.Contains(SelectedMemberToRemove))
	            {
	                Members.Add(SelectedMemberToRemove);
	            }
	        }

	        if (SelectedMembers.Contains(SelectedMemberToRemove))
	        {
	            SelectedMembers.Remove(SelectedMemberToRemove);
	        }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
	    {
	        try
            {
                if (parameters.ContainsKey(nameof(SelectedCourseSelectionInfo)))
                {
                    SelectedCourseSelectionInfo = parameters[nameof(SelectedCourseSelectionInfo)].ToString()
                        .Deserialize<CourseSelectionInfo>();
                }

                await Filter();
            }
            catch (Exception e)
	        {
	            _logger.LogError(e.ToString());
	        }
	    }

        private async System.Threading.Tasks.Task Filter()
        {
            try
            {
                Members.Clear();

                if (_allMembers.Count == 0)
                {
                    _allMembers = await _memberRepository.GetAsync();
                }

                Func<Mitglied, string> orderFirstname = m => m.Name;

                Func<Mitglied, bool> onlyActiveMembersExpression = m => m.IsActive();

                IEnumerable<Mitglied> filteredMembers = _allMembers;

                if (OnlyActiveMembers)
                {
                    filteredMembers = filteredMembers.Where(onlyActiveMembersExpression);
                }

                if (OnlyTrainers)
                {
                    if (_trainerAppointments.Count == 0)
                    {
                        _trainerAppointments = await _trainerAppointmentsRepository.GetAsync();
                    }

                    var trainerMemberIds = _trainerAppointments.Select(t => t.MitgliedID).Distinct();

                    Func<Mitglied, bool> onlyTrainersExpression = m => trainerMemberIds.Contains(m.Id);

                    filteredMembers = filteredMembers.Where(onlyTrainersExpression);
                }

                var withoutChildren = !WithChildren;
                var withoutTeeangers = !WithTeenagers;
                var withoutAdults = !WithAdults;

                if (withoutChildren)
                {
                    //filteredMembers = filteredMembers.Where(m=>m.GetAge() < 14);
                    filteredMembers = filteredMembers.Where(m => m.GetAge() > 13);
                }

                if (withoutTeeangers)
                {
                    //filteredMembers = filteredMembers.Where(m => m.GetAge() > 13 && m.GetAge() < 18);
                    filteredMembers = filteredMembers.Where(m => m.GetAge() < 14 || m.GetAge() > 17);
                }

                if (withoutAdults)
                {
                    //filteredMembers = filteredMembers.Where(m => m.GetAge() > 17);
                    filteredMembers = filteredMembers.Where(m => m.GetAge() < 18);
                }

                if (_onlyLastAttendees)
                {
                    var filter = new Filter
                    {
                        ColumnName = nameof(Training.KursID),
                        MatchType = "eq",
                        Value = SelectedCourseSelectionInfo.CourseId.ToString()
                    };

                    var trainingTerminStart = (DateTime.Now -TimeSpan.FromDays(31*OnlyLastAttendeesMonths)).ToString("s");
                    var filter2 = new Filter
                    {
                        ColumnName = nameof(Training.Termin),
                        MatchType = "ge",
                        Value = trainingTerminStart
                    };

                    var filters = new List<Filter> {filter, filter2};

                    var trainings = await _trainingsRepository.GetAsync(filters);
                    _logger.LogTrace(trainings.Count.ToString());
                }

                var orderedMembers = filteredMembers.OrderBy(orderFirstname);

                foreach (var member in orderedMembers)
                {
                    var memberInfo = new MemberInfo { FirstName = member.FirstName, LastName = member.Nachname };
                    Members.Add(memberInfo);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }
    }
}
