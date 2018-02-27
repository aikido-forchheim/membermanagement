using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

// CourseAxis bundles everything VerticalAxisCourses and HorizontalAxisCourses have in common

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeCourse : AxisType
    {
        public AxisTypeCourse()
        {
            P_ActiveElementsOnly = false;
        }

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idKurs, bool action)
            => action
               ? ReportTrainingsParticipation.Show(new ReportMemberVsTrainings(datStart, datEnd, idKurs))
               : $"Klicken für Details zum Kurs\n" + GetDescription(idKurs);

        public override int P_MaxDbId { get; } = Globals.DatabaseWrapper.MaxKursNr();

        public override int P_MinDbId { get; } = Globals.DatabaseWrapper.MinKursNr();

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);

        public override string GetDescription(int idKurs)
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
    }
}
