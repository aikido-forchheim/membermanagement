using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMonth : AxisType
    {
        private DateTime m_datStart;
        private DateTime m_datEnd;

        public AxisTypeMonth(DateTime datStart, DateTime datEnd)
        {
            P_ActiveElementsOnly = false;
            m_datStart = datStart;
            m_datEnd = datEnd;
            P_MinDbId = 0;
            P_MaxDbId = NrOfMonths(datStart, datEnd);
            HeaderStrings = new List<string> { "Monat" };
        }

        private int NrOfMonths(DateTime datStart, DateTime datEnd)
            => (datEnd.Year - datStart.Year) * 12 + (datEnd.Month - datStart.Month);

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.m_monat.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => iModelIndex;

        private DateTime FirstDayOfMonth(int idMonth)
            => new DateTime(m_datStart.Year, m_datStart.Month + idMonth, 1);

        private DateTime LastDayOfMonth(int idMonth)
            => FirstDayOfMonth( idMonth ).AddDays(DateTime.DaysInMonth(m_datStart.Year, m_datStart.Month) - 1);

        public override string MouseAxisEvent(DateTime datStart, DateTime datEnd, int idMonth, bool action)
            => action
               ? ReportMain.SwitchToPanel(new ReportWeeksVsCourses(FirstDayOfMonth(idMonth), LastDayOfMonth(idMonth), idMonth))
               : $"Klicken für Details zum Monat " + GetDescription(idMonth);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfMonths(m_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idMonth, int iNr = 1)
            => GetDesc(idMonth, m_datStart, iNr);

        public static string GetDesc(int id, DateTime datStart, int iNr = 1)
        {
            int    nrOfMonths    = (datStart.Month - 1) + id;
            int    year          = datStart.Year + nrOfMonths / 12;
            Monat  monat         = Globals.DatabaseWrapper.MonatFromId(nrOfMonths % 12);
            string strMonthShort = monat.Bezeichnung.Substring(0, 3);
            return $"{strMonthShort} {year}";
        }
    }
}
