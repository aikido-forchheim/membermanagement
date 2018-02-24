using System;
using System.Diagnostics;
using System.Globalization;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Globals
    {
        private static DateTimeFormatInfo m_dfi;
        private static Calendar m_cal;

        public static DatabaseWrapper DatabaseWrapper;

        public static void Initialize(IUnityContainer Container)
        {
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
            DatabaseWrapper = new DatabaseWrapper();
            DatabaseWrapper.ReadTables(Container).Wait();
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
            if (idKurs == 0)
            {
                return "Lehrg.\netc.";
            }
            else
            {
                Kurs kurs = Globals.DatabaseWrapper.KursFromId(idKurs);
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
