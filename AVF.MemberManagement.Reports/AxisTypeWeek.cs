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
                    xAxisType: typeof(AxisTypeCourse),
                    yAxisType: typeof(AxisTypeMember),
                    period: ReportDescriptor.Period.WEEK,
                    nrPeriod: nrWeek
                 )
               : $"Klicken für Details zu " + HeaderStrings[0] + " " + GetFullDesc(nrWeek, Globals.TEXT_ORIENTATION.HORIZONTAL);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfWeeks(P_reportDescriptor.P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override List<string> GetDescription(int nrWeek, Globals.TEXT_ORIENTATION o)
        {
            string strWeek = $"{Globals.GetWeekOfYear(P_reportDescriptor.P_timeRange.P_datStart, nrWeek)}";
            string strYear = $"{P_reportDescriptor.P_timeRange.P_datStart.Year}";

            List<string> list = new List<string>();
            switch (o)
            {
                case Globals.TEXT_ORIENTATION.VERTICAL:
                    list.Add($"KW\n{strWeek}\n{strYear}");
                    break;

                case Globals.TEXT_ORIENTATION.HORIZONTAL:
                    list.Add($"{strWeek}/{strYear}");
                    break;

                case Globals.TEXT_ORIENTATION.SPECIAL:
                    list.Add(strWeek);
                    list.Add(strYear);
                    break;
            }

            return list;
        }
    }
}
