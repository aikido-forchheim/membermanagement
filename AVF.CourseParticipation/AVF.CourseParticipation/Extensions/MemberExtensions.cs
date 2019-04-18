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

        public static int GetAge(this Mitglied m)
        {
            return CalculateAge(m.Geburtsdatum);
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
    }
}
