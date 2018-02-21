using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;
using Prism.Navigation;

namespace AVF.MemberManagement.ViewModels
{
    public class EnterParticipantsTabletPageViewModel : EnterParticipantsPageViewModel
    {
        public EnterParticipantsTabletPageViewModel(IRepository<Mitglied> mitgliederRepository, IRepository<Training> trainingsRepository, IRepository<TrainingsTeilnahme> trainingsTeilnahmenRepository, INavigationService navigationService, ILogger logger) : base(mitgliederRepository, trainingsRepository, trainingsTeilnahmenRepository, navigationService, logger)
        {
        }
    }
}
