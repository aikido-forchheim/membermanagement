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
                    yAxisType: typeof(AxisTypeMonth),
                    period: ReportDescriptor.Period.YEAR,
                    nrPeriod: idYear
                )
              : $"Klicken für Details zu " + HeaderStrings[0] + " " + GetFullDesc(idYear, " ");

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfYears(P_reportDescriptor.P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override List<string> GetDescription(int idYear)
        {
            List<string> list = new List<string>();
            list.Add($"{P_reportDescriptor.P_timeRange.P_datStart.Year + idYear}");
            return list;
        }
    }
}
