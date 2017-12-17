using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ViewModels
{
    public class EnterParticipantsTabletPageViewModel : EnterParticipantsPageViewModel
    {
        public EnterParticipantsTabletPageViewModel(IRepository<Mitglied> mitgliederRepository, IRepository<Training> trainingsRepository, IRepository<TrainingsTeilnahme> trainingsTeilnahmenRepository) : base(mitgliederRepository, trainingsRepository, trainingsTeilnahmenRepository)
        {
        }
    }
}
