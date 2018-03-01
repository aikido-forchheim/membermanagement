using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeCourse : AxisType
    {
        public AxisTypeCourse()
        { 
            P_ActiveElementsOnly = false;
            P_MaxDbId = Globals.DatabaseWrapper.MaxKursNr();
            P_MinDbId = Globals.DatabaseWrapper.MinKursNr();
        }

        public override VerticalAxis GetVerticalAxis()
            => new VerticalAxisCourses();

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.m_kurs.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => Globals.DatabaseWrapper.m_kurs[iModelIndex].Id;

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idKurs, bool action)
            => action
               ? ReportTrainingsParticipation.Show(new ReportMemberVsTrainings(datStart, datEnd, idKurs))
               : $"Klicken für Details zum Kurs\n" + GetDescription(idKurs, ' ');

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);

        public override string GetDescription(int idKurs, char separator)
            => GetDesc(idKurs, separator);

        public static string GetDesc(int idKurs, char separator)
        {
            Kurs kurs = Globals.DatabaseWrapper.KursFromId(idKurs);

            if (kurs.Zeit == TimeSpan.Zero)
            {
                return $"_Lehrg.{separator}etc.";
            }
            else
            {
                string day = Globals.DatabaseWrapper.WeekDay(kurs.WochentagID).Substring(0, 2);
                return $"{ day }{separator}{kurs.Zeit:hh}:{kurs.Zeit:mm}";
            }
        }
    }
}
