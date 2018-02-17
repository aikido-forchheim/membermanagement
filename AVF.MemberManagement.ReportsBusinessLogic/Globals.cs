using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Globals
    {
        public static DatabaseWrapper DatabaseWrapper;

        public static void Initialize(IUnityContainer Container)
        {
            DatabaseWrapper = new DatabaseWrapper();
            DatabaseWrapper.ReadTables(Container).Wait();
        }

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
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            return $"{training.Termin:dd}.\n{training.Termin:MM}.";
        }

        public static string GetTimeRangeDescription(DateTime datStart, DateTime datEnd)
        {
            return $"{datStart:dd}.{datStart:MM}.{datStart:yyyy} {datEnd:dd}.{datEnd:MM}.{datEnd:yyyy}";
        }

        public static string GetMemberDescription( int idMember )
        {
            Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(idMember);
            return $"Trainingsteilnahme {mitglied.Vorname} {mitglied.Nachname} MitgliedNr. {mitglied.Id}";
        }

    }
}
