using System;
using System.Collections.Generic;
using System.Text;

namespace AVF.CourseParticipation.Models
{
    public class MemberInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;
    }
}
