using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class AxisTypeWeek : AxisType
    {
        protected DateTime m_datStart;
        protected DateTime m_datEnd;

        public AxisTypeWeek(DateTime datStart, DateTime datEnd)
        {
            P_ActiveElementsOnly = false;
            m_datStart = datStart;
            m_datEnd   = datEnd;
            P_MinDbId  = 0;
            P_MaxDbId  = NrOfWeeks(datStart, datEnd);
            HeaderStrings = new List<string> { "KW" };
        }

        private int NrOfWeeks(DateTime datStart, DateTime datEnd)
        {
            int weekStart = Globals.GetWeekOfYear(datStart);
            int weekEnd   = Globals.GetWeekOfYear(datEnd);
            if (weekStart > weekEnd)
                weekStart = 0;
            int id =  weekEnd - weekStart;
            return id;
        }

        public override int GetModelIndexFromId(int id)
            => id;

        public override int GetIdFromModelIndex(int iModelIndex)
            => iModelIndex;

        public override string MouseAxisEvent(DateTime datStart, DateTime datEnd, int idWeek, bool action)
           => action
               ? ReportMain.SwitchToPanel(ReportWeek.GetReport( datStart.Year, Globals.GetWeekOfYear(datStart) + idWeek ))
               : $"Klicken für Details zur Woche " + GetDescription(idWeek);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfWeeks(m_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idWeek, int iNr = 1)
            => $"{Globals.GetWeekOfYear(m_datStart) + idWeek} {m_datStart.Year}";
    }
}

