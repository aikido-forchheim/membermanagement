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
using Prism.Services;
using Xamarin.Forms;

namespace AVF.CourseParticipation.ViewModels
{
	public class MemberSelectionPageViewModel : ViewModelBaseLoggedIn
	{
	    protected readonly IRepository<Mitglied> MemberRepository;
	    protected readonly IRepository<Training> TrainingsRepository;

        private readonly ILogger _logger;
	    private readonly IRepository<TrainerErnennung> _trainerAppointmentsRepository;
	    private readonly IRepository<TrainingsTeilnahme> _trainingParticipationsRepository;
	    private readonly IPageDialogService _dialogService;

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
	    private bool _onlyLastAttendeesByDefault;

	    public CourseSelectionInfo SelectedCourseSelectionInfo
	    {
	        get => _selectedCourseSelectionInfo;
	        set => SetProperty(ref _selectedCourseSelectionInfo, value);
	    }

	    private DateTime _selectedDate = DateTime.Today;

	    public DateTime SelectedDate
	    {
	        get => _selectedDate;
	        set => SetProperty(ref _selectedDate, value);
	    }

        private string _searchText;

	    public string SearchText
	    {
	        get => _searchText;
	        set
	        {
	            SetProperty(ref _searchText, value);
	            Filter().IgnoreResult();
            }
	    }

	    public ICommand AddSelectedMemberCommand { get; }
	    public ICommand RemoveSelectedMemberCommand { get; }

        public MemberSelectionPageViewModel(INavigationService navigationService, IRepository<Mitglied> memberRepository, ILogger logger, IRepository<TrainerErnennung> trainerAppointmentsRepository, IRepository<Training> trainingsRepository, IRepository<TrainingsTeilnahme> trainingParticipationsRepository, IPageDialogService dialogService) : base(navigationService)
	    {
	        MemberRepository = memberRepository;
	        _logger = logger;
	        _trainerAppointmentsRepository = trainerAppointmentsRepository;
	        TrainingsRepository = trainingsRepository;
	        _trainingParticipationsRepository = trainingParticipationsRepository;
	        _dialogService = dialogService;

	        AddSelectedMemberCommand = new DelegateCommand(AddSelectedMember, CanAddSelectedMember).ObservesProperty(() => SelectedMember);
	        RemoveSelectedMemberCommand = new DelegateCommand(RemoveSelectedMember, CanRemoveSelectedMember).ObservesProperty(() => SelectedMemberToRemove);
        }

        private bool CanAddSelectedMember()
        {
            return SelectedMember != null;
        }

	    private void AddSelectedMember()
        {
            var member = SelectedMember;

            AddSelectedMember(member);
        }

        protected void AddSelectedMember(MemberInfo member)
        {
            if (member != null)
            {
                if (!SelectedMembers.Contains(member))
                {
                    SelectedMembers.Add(member);
                }
            }

            if (Members.Contains(member))
            {
                Members.Remove(member);
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
            base.OnNavigatedTo(parameters);

            await OnNavigatedToAsync(parameters);
        }

        protected async System.Threading.Tasks.Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            try
            {
                if (parameters.ContainsKey(nameof(SelectedDate)))
                {
                    if (DateTime.TryParse(parameters[nameof(SelectedDate)].ToString(), out var parsedDate))
                    {
                        SelectedDate = parsedDate;
                    }
                }

                if (parameters.ContainsKey(nameof(SelectedCourseSelectionInfo)))
                {
                    SelectedCourseSelectionInfo = (CourseSelectionInfo)parameters[nameof(SelectedCourseSelectionInfo)];
                }

                if (Application.Current.Properties.ContainsKey("OnlyLastAttendeesByDefault")) //need to use string, only private
                {
                    _onlyLastAttendeesByDefault = (bool)Application.Current.Properties["OnlyLastAttendeesByDefault"];
                }

                OnlyLastAttendees = _onlyLastAttendeesByDefault; //ATTENTION: This alread triggers the filter, be careful when adding more parameters like this

                //await Filter();
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
                    _allMembers = await MemberRepository.GetAsync();
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

                if (_onlyLastAttendees && string.IsNullOrWhiteSpace(SearchText))
                {
                    var filterTrainingByCourseId = new Filter
                    {
                        ColumnName = nameof(Training.KursID),
                        MatchType = "eq",
                        Value = SelectedCourseSelectionInfo.CourseId.ToString()
                    };

                    var trainingTerminStart = (DateTime.Now -TimeSpan.FromDays(31*OnlyLastAttendeesMonths)).ToString("s");
                    var filterTrainingByDate = new Filter
                    {
                        ColumnName = nameof(Training.Termin),
                        MatchType = "ge",
                        Value = trainingTerminStart
                    };

                    var trainings = await TrainingsRepository.GetAsync(new List<Filter> { filterTrainingByCourseId, filterTrainingByDate });
                    _logger.LogTrace(trainings.Count.ToString());

                    var foundTrainingParticipationsMemberIds = new List<int>();
                    foreach (var training in trainings)
                    {
                        var filterTrainingsTeilnahmeByTrainingId = new Filter
                        {
                            ColumnName = nameof(TrainingsTeilnahme.TrainingID),
                            MatchType = "eq",
                            Value = training.Id.ToString()
                        };

                        var trainingParticipations = (await _trainingParticipationsRepository.GetAsync(new List<Filter> { filterTrainingsTeilnahmeByTrainingId })).ToList();

                        foreach (var trainingParticipation in trainingParticipations)
                        {
                            if (!foundTrainingParticipationsMemberIds.Contains(trainingParticipation.MitgliedID))
                            {
                                foundTrainingParticipationsMemberIds.Add(trainingParticipation.MitgliedID);
                            }
                        }
                    }

                    filteredMembers = filteredMembers.Where(m=>foundTrainingParticipationsMemberIds.Contains(m.Id));
                }

                if (!string.IsNullOrWhiteSpace(SearchText) && SearchText.Length > 1)
                {
                    var foundMemberyByName = new List<Mitglied>();
                    foreach (var member in _allMembers)
                    {
                        if (member.FirstName.ToLower().Contains(SearchText.ToLower()) || member.Nachname.ToLower().Contains(SearchText.ToLower()) || member.Vorname.ToLower().Contains(SearchText.ToLower()))
                        {
                            if (!foundMemberyByName.Contains(member))
                            {
                                foundMemberyByName.Add(member);
                            }
                        }
                    }

                    filteredMembers = filteredMembers.Where(m => foundMemberyByName.Contains(m));
                }

                var orderedMembers = filteredMembers.OrderBy(orderFirstname);

                foreach (var member in orderedMembers)
                {
                    var memberInfo = new MemberInfo { MemberId = member.Id, FirstName = member.FirstName, LastName = member.Nachname };
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
