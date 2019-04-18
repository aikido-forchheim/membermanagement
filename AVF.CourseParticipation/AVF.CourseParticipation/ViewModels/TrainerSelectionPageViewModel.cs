using System;
using System.Collections.Generic;
using System.Text;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;
using Prism.Navigation;

namespace AVF.CourseParticipation.ViewModels
{
    public class TrainerSelectionPageViewModel : MemberSelectionPageViewModel
    {
        public TrainerSelectionPageViewModel(INavigationService navigationService, IRepository<Mitglied> memberRepository, ILogger logger, IRepository<TrainerErnennung> trainerAppointmentsRepository) : base(navigationService, memberRepository, logger, trainerAppointmentsRepository)
        {
        }
    }
}
