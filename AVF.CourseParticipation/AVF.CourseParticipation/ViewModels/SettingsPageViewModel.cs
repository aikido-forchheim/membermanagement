using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AVF.CourseParticipation.ViewModels
{
	public class SettingsPageViewModel : ViewModelBase
	{
        private readonly IRepository<User> _usersRepository;

        public SettingsPageViewModel(INavigationService navigationService, IRepository<User> usersRepository)
         : base(navigationService)
        {
            _usersRepository = usersRepository;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

        }
    }
}
