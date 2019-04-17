using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

	    private ObservableCollection<MemberInfo> _members = new ObservableCollection<MemberInfo>();

	    public ObservableCollection<MemberInfo> Members
	    {
	        get => _members;
	        set => SetProperty(ref _members, value);
	    }

	    public TrainerSelectionPageViewModel(INavigationService navigationService, IRepository<Mitglied> memberRepository, ILogger logger) : base(navigationService)
	    {
	        _memberRepository = memberRepository;
	        _logger = logger;
	    }

	    public override async void OnNavigatedTo(INavigationParameters parameters)
	    {
	        try
	        {
                Members.Clear();

	            var members = await _memberRepository.GetAsync();
	            var orderedMembers = members.Where(m => m.Austritt == null || m.Austritt > DateTime.Now).OrderBy(m => m.Name);
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
