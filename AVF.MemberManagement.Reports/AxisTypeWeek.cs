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
        }

        private int NrOfWeeks(DateTime datStart, DateTime datEnd)
            => (datEnd - datStart).Days / 7;

        public override string MouseAxisEvent(int nrWeek, bool action)
           => action
               ? ReportMain.P_formMain.NewTrainingsParticipationPanel
                 (
                    defaultDesc: P_reportDescriptor,
                    yAxisType: typeof(AxisTypeTraining),
                    period: ReportDescriptor.Period.WEEK,
                    nrPeriod: nrWeek
                 )
               : $"Klicken für Details zu " + HeaderStrings[0] + " " + GetFullDesc(nrWeek, " ");

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfWeeks(P_reportDescriptor.P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override List<string> GetDescription(int nrWeek)
        {
            List<string> list = new List<string>();
            list.Add($"{Globals.GetWeekOfYear(P_reportDescriptor.P_timeRange.P_datStart, nrWeek)}/{P_reportDescriptor.P_timeRange.P_datStart.Year}");
            return list;
        }
    }
}
