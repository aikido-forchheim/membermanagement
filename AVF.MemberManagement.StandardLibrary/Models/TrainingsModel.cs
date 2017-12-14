using System;
using System.Collections.Generic;
using System.Text;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class TrainingsModel
    {
        public Class Class { get; set; }

        public Training Training { get; set; }

        public List<TrainingsTeilnahme> Participations { get; set; }
    }
}
