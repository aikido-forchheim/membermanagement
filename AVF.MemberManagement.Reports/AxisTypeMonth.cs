using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMonth : AxisType
    {
        public AxisTypeMonth()
            => P_ActiveElementsOnly = false;

        private const int LAST_MONTH = 12;
        private const int FRST_MONTH = 1;

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idMonth, bool action)
            => action
               ? String.Empty
               : $"Klicken für Details zum Monat " + GetDescription(idMonth) + "\n noch nicht implementiert";

        public override int P_MaxDbId { get; } = LAST_MONTH;

        public override int P_MinDbId { get; } = FRST_MONTH;

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.GetMonth(Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idMonth)
            => Globals.GetMonthName(idMonth);
    }
}
