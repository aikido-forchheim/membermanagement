using System;
using System.Collections.Generic;
using System.Text;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.CourseParticipation.Models
{
    public class MemberInfo
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;

        public MemberInfo()
        {

        }

        public MemberInfo(Mitglied member)
        {
            MemberId = member.Id;
            FirstName = member.FirstName;
            LastName = member.Nachname;
        }
    }
}
