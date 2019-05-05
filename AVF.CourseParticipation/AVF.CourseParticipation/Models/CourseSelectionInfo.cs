using System;
using System.Collections.Generic;

namespace AVF.CourseParticipation.Models
{
    public class CourseSelectionInfo
    {
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int MemberId { get; set; }

        public List<int?> ContrainerMemberIds { get; set; } = new List<int?>();

        public int Participants { get; set; }
        public int CourseId { get; set; }
        public bool ChildrensTraining { get; set; }
        public TimeSpan Duration { get; set; }
    }
}