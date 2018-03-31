using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class AxisTypeWeek : AxisTypeTime
    {
        public AxisTypeWeek(ReportDescriptor desc)
             : base(desc)
        {
            P_MaxDbId  = NrOfWeeks(P_reportDescriptor.P_timeRange.P_datStart, P_reportDescriptor.P_timeRange.P_datEnd);
            HeaderStrings = new List<string> { "KW" };
            m_period = Period.WEEK;
        }

        private int NrOfWeeks(DateTime datStart, DateTime datEnd)
        {
            int weekStart = Globals.GetWeekOfYear(datStart);
            int weekEnd = Globals.GetWeekOfYear(datEnd);
            if (weekStart > weekEnd)
                weekStart = 0;
            int id = weekEnd - weekStart;
            return id;
        }

        public override string MouseAxisEvent(int idWeek, bool action)
           => action
               ? ReportMain.P_formMain.NewTrainingsParticipationPanel
                 (
                    defaultDesc: P_reportDescriptor,
                    yAxisType: typeof(AxisTypeTraining), 
                    idWeek: idWeek
                 )
               : $"Klicken für Details zu " + HeaderStrings[0] + " " + GetDescription(idWeek);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfWeeks(P_reportDescriptor.P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override List<string> GetDescription(int idWeek)
        {
            List<string> list = new List<string>();
            list.Add($"{Globals.GetWeekOfYear(P_reportDescriptor.P_timeRange.P_datStart, idWeek)}/{P_reportDescriptor.P_timeRange.P_datStart.Year}");
            return list;
        }
    }
}
