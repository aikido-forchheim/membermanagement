using System;
using System.Diagnostics;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeWeek : AxisType
    {
        private const int LAST_CALENDAR_WEEK = 52;
        private const int FRST_CALENDAR_WEEK = 1;

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int id, bool action)
            => action
               ? String.Empty
               : $"Klicken für Details zu Trainings in dieser Woche (Noch nicht implementiert)";

        public override int P_MaxDbId { get; } = LAST_CALENDAR_WEEK;

        public override int P_MinDbId { get; } = FRST_CALENDAR_WEEK;

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
        {
            Debug.Assert(tn.TrainingID > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(tn.TrainingID);
            return Globals.GetWeekOfYear(training.Termin);
        }
    }
}
