using System;
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
        }

        public override VerticalAxis GetVerticalAxis()
            => new VerticalAxisWeeks(m_datStart, m_datEnd);

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
               : $"Klicken für Details zur Woche " + GetDescription(idWeek, '/');

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfWeeks(m_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idWeek, char separator)
            => GetDesc(idWeek, separator, m_datStart);

        public static string GetDesc(int id, char separator, DateTime datStart)
            => $"{Globals.GetWeekOfYear(datStart) + id}{separator}{datStart.Year}";
    }
}

