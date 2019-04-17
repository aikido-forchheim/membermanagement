using System;
using System.Collections.Generic;
using System.Text;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.CourseParticipation.Extensions
{
    public static class MemberExtensions
    {
        public static bool IsActive(this Mitglied m)
        {
            return (m.Austritt == null || m.Austritt > DateTime.Now) && m.Id > 0 && m.BeitragsklasseID != 3;
        }
    }
}
