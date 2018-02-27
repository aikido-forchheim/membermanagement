using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeWeek : AxisType
    {
        public AxisTypeWeek(DateTime datStart, DateTime datEnd)
        { 
            P_ActiveElementsOnly = false;
            P_MaxDbId = LAST_CALENDAR_WEEK;
            P_MinDbId = FRST_CALENDAR_WEEK;
        }
        
        private const int LAST_CALENDAR_WEEK = 52;
        private const int FRST_CALENDAR_WEEK = 1;

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int id, bool action)
            => action
               ? String.Empty
               : $"Klicken für Details zu Trainings in dieser Woche (Noch nicht implementiert)";

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.GetWeekOfYear(Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idWeek)
            => String.Empty;
    }
}
