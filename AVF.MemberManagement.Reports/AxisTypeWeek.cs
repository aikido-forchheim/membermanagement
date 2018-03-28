using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class AxisTypeWeek : AxisType
    {
        public AxisTypeWeek(TimeRange timeRange)
             : base(timeRange)
        {
            P_ActiveElementsOnly = false;
            P_MinDbId  = 0;
            P_MaxDbId  = NrOfWeeks(timeRange.P_datStart, timeRange.P_datEnd);
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

        public override string MouseAxisEvent(int idWeek, bool action)
           => action
               ? ReportMain.P_formMain.SwitchToPanel(ReportWeek.GetReport(P_timeRange.P_datStart.Year, Globals.GetWeekOfYear(P_timeRange.P_datStart, idWeek )))
               : $"Klicken für Details zur Woche " + GetDescription(idWeek);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfWeeks(P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idWeek, int iNr = 1)
            => $"{Globals.GetWeekOfYear(P_timeRange.P_datStart, idWeek)}/{P_timeRange.P_datStart.Year}";
    }
}

