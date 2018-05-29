using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class AxisTypeYear : AxisTypeTime
    {
        public AxisTypeYear(ReportDescriptor desc)
             : base(desc)
        {
            P_MaxDbId = NrOfYears(P_reportDescriptor.P_timeRange.P_datStart, P_reportDescriptor.P_timeRange.P_datEnd);
            HeaderStrings = new List<string> { "Jahr" };
        }

        private int NrOfYears(DateTime datStart, DateTime datEnd)
            => datEnd.Year - datStart.Year;

        public override string MouseAxisEvent(int idYear, bool action)
           => action
              ? ReportMain.P_formMain.NewTrainingsParticipationPanel
                (
                    defaultDesc: P_reportDescriptor,
                    xAxisType: (P_reportDescriptor.P_xAxisType == typeof(AxisTypeYear)) ? typeof(AxisTypeMonth) : P_reportDescriptor.P_xAxisType,
                    yAxisType: (P_reportDescriptor.P_yAxisType == typeof(AxisTypeYear)) ? typeof(AxisTypeMonth) : P_reportDescriptor.P_yAxisType,
                    period: ReportDescriptor.Period.YEAR,
                    nrPeriod: idYear
                )
              : $"Klicken für Details zu " + HeaderStrings[0] + " " + GetFullDesc(idYear, Globals.TEXT_ORIENTATION.HORIZONTAL);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfYears(P_reportDescriptor.P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override List<string> GetDescription(int idYear, Globals.TEXT_ORIENTATION o)
        {
            List<string> list = new List<string>();
            list.Add($"{P_reportDescriptor.P_timeRange.P_datStart.Year + idYear}");
            return list;
        }
    }
}
