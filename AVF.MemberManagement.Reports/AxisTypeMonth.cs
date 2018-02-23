using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMonth : AxisType
    {
        private const int LAST_MONTH = 12;
        private const int FRST_MONTH = 1;

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idMonth, bool action)
            => action
               ? String.Empty
               : $"Klicken für Details zum Monat " + Globals.GetMonthName(idMonth) + "\n noch nicht implementiert";

        public override int P_MaxDbId { get; } = LAST_MONTH;

        public override int P_MinDbId { get; } = FRST_MONTH;

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;
    }
}
