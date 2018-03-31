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
            HeaderStrings = new List<string> { "Monat" };
            m_period = Period.MONTH;
        }

        private int NrOfMonths(DateTime datStart, DateTime datEnd)
            => (datEnd.Year - datStart.Year) * 12 + (datEnd.Month - datStart.Month);

        public override string MouseAxisEvent(int idMonth, bool action)
            => action
               ? ReportMain.P_formMain.NewTrainingsParticipationPanel
                 (
                    defaultDesc: P_reportDescriptor,
                    yAxisType: typeof(AxisTypeWeek), 
                    idMonth: idMonth
                 )
               : $"Klicken für Details zu " + HeaderStrings[0] + " " + GetDescription(idMonth);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfMonths(P_reportDescriptor.P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override List<string> GetDescription(int idMonth)
            => GetDesc(P_reportDescriptor, idMonth);

        public static List<string> GetDesc( ReportDescriptor desc, int idMonth )
        {
            List<string> list = new List<string>();
            if (idMonth == Globals.ALL_MONTHS)
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
