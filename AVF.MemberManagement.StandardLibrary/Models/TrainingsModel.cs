using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class TrainingsModel
    {
        public Class Class { get; set; }

        public Training Training { get; set; }

        public IEnumerable<TrainingsTeilnahme> Participations { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }
    }
}
