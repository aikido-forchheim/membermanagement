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
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;

namespace AVF.CourseParticipation.ViewModels
{
	public class TrainerSelectionPageViewModel : ViewModelBase
	{
	    private readonly IRepository<Mitglied> _memberRepository;
	    private readonly ILogger _logger;

	    private static List<Mitglied> _allMembers = new List<Mitglied>();

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

        public ICommand AddSelectedMemberCommand { get; }
	    public ICommand RemoveSelectedMemberCommand { get; }

        public TrainerSelectionPageViewModel(INavigationService navigationService, IRepository<Mitglied> memberRepository, ILogger logger) : base(navigationService)
	    {
	        _memberRepository = memberRepository;
	        _logger = logger;

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
                Members.Clear();

	            if (_allMembers.Count == 0)
	            {
	                _allMembers = await _memberRepository.GetAsync();
	            }

	            Func<Mitglied, string> firstNameOrder = m => m.Name;

	            var orderedMembers = _allMembers.Where(m => m.IsActive()).OrderBy(firstNameOrder);
	            foreach (var member in orderedMembers)
	            {
	                var memberInfo = new MemberInfo {FirstName = member.FirstName, LastName = member.Nachname};
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
