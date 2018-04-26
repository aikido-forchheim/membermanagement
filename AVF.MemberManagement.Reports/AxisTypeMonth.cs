using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMonth : AxisTypeTime
    {
        public AxisTypeMonth(ReportDescriptor desc)
             : base(desc)
        {
            P_MaxDbId = NrOfMonths(P_reportDescriptor.P_timeRange.P_datStart, P_reportDescriptor.P_timeRange.P_datEnd);
            HeaderStrings = new List<string> { "Monat", "Jahr" };
        }

        private int NrOfMonths(DateTime datStart, DateTime datEnd)
            => (datEnd.Year - datStart.Year) * 12 + (datEnd.Month - datStart.Month);

        public override string MouseAxisEvent(int idMonth, bool action)
            => action
               ? ReportMain.P_formMain.NewTrainingsParticipationPanel
                 (
                    defaultDesc: P_reportDescriptor,
                    xAxisType: (P_reportDescriptor.P_xAxisType == typeof(AxisTypeMonth)) ? typeof(AxisTypeWeek) : P_reportDescriptor.P_xAxisType,
                    yAxisType: (P_reportDescriptor.P_yAxisType == typeof(AxisTypeMonth)) ? typeof(AxisTypeWeek) : P_reportDescriptor.P_yAxisType,
                    period: ReportDescriptor.Period.MONTH,
                    nrPeriod: idMonth
                 )
               : $"Klicken für Details zu " + HeaderStrings[0] + " " + GetFullDesc(idMonth, Globals.TEXT_ORIENTATION.HORIZONTAL);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfMonths(P_reportDescriptor.P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override List<string> GetDescription(int idMonth, Globals.TEXT_ORIENTATION o)
            => GetDesc(P_reportDescriptor, idMonth);

        public static List<string> GetDesc( ReportDescriptor desc, int idMonth )
        {
            List<string> list = new List<string>();
            if (idMonth == Globals._UNDEFINED)
            {
                list.Add(string.Empty);
            }
            else
            {
                int nrOfMonths = (desc.P_timeRange.P_datStart.Month - 1) + idMonth;
                int year = desc.P_timeRange.P_datStart.Year + nrOfMonths / 12;
                string strMonat = Globals.GetMonthName(nrOfMonths % 12 + 1);
                string strMonthShort = strMonat.Substring(0, 3);
                list.Add($"{strMonthShort}");
                list.Add($"{year}");
            }
            return list;
        }
    }
}
