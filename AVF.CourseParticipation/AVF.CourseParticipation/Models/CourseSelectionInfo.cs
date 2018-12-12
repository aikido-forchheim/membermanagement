using System;

namespace AVF.CourseParticipation.Models
{
    public class CourseSelectionInfo
    {
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Participants { get; set; }
    }
}