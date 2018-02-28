using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMonth : AxisType
    {
        DateTime m_datStart;

        public AxisTypeMonth(DateTime datStart, DateTime datEnd)
        {
            P_ActiveElementsOnly = false;
            m_datStart = datStart;
            P_MinDbId = 0;
            P_MaxDbId = NrOfMonths(datStart, datEnd);
        }

        private int NrOfMonths(DateTime datStart, DateTime datEnd)
            => (datEnd.Year - datStart.Year) * 12 + (datEnd.Month - datStart.Month);

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.m_monat.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => iModelIndex; // Globals.DatabaseWrapper.m_monat[iModelIndex % 12].Id;

         public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idMonth, bool action)
            => action
               ? String.Empty
               : $"Klicken für Details zum Monat " + GetDescription(idMonth, ' ') + "\nnoch nicht implementiert";

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfMonths(m_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idMonth, char separator)
            => GetDesc(idMonth, separator, m_datStart);

        public static string GetDesc(int id, char separator, DateTime datStart)
        {
            int   x     = (datStart.Month - 1) + id;
            int   year  = datStart.Year + x / 12;
            Monat monat = Globals.DatabaseWrapper.MonatFromId(x % 12);
            string strMonthShort = monat.Bezeichnung.Substring(0, 3);
            return $"{strMonthShort}{separator}{year}";
        }
    }
}
