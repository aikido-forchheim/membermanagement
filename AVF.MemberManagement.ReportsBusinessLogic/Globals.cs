using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Globals
    {
        private static DateTimeFormatInfo m_dfi;
        private static Calendar m_cal;

        public static DatabaseWrapper DatabaseWrapper;

        public async static Task Initialize(IUnityContainer Container, Action<String> tick)
        {
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
            DatabaseWrapper = new DatabaseWrapper();
            await DatabaseWrapper.ReadTables(Container, tick);
        }

        public static string Format(DateTime date)
            => $"{ date:dd.MM.yyyy}";

        public static int GetMonth(DateTime date)
            => m_cal.GetMonth( date );

        public static string GetMonthName(int iMonth)
            => m_dfi.GetMonthName(iMonth).ToString();

        public static int GetWeekOfYear(DateTime date)
            => m_cal.GetWeekOfYear(date, m_dfi.CalendarWeekRule, m_dfi.FirstDayOfWeek);

        public static string GetTimeRangeDescription(DateTime datStart, DateTime datEnd)
            => Format(datStart) + " " + Format(datEnd);

        public static string GetCourseDescription(int idKurs)
        {
            Kurs kurs = Globals.DatabaseWrapper.KursFromId(idKurs);

            if (kurs.Zeit == TimeSpan.Zero)
            {
                return "Lehrg.\netc.";
            }
            else
            {
                string day = Globals.DatabaseWrapper.WeekDay(kurs.WochentagID).Substring(0, 2);
                return $"{ day }\n{kurs.Zeit:hh}:{kurs.Zeit:mm}";
            }
        }

        public static string GetTrainingDescription(int idTraining)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            return $"{training.Termin:dd}\n{training.Termin:MM}";
        }

        public static string GetMemberDescription( int idMember )
        {
            Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(idMember);
            return $"{mitglied.Vorname} {mitglied.Nachname} MitgliedNr. {mitglied.Id}";
        }

    }
}
