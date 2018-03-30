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
            m_period = Period.YEAR;
        }

        private int NrOfYears(DateTime datStart, DateTime datEnd)
            => datEnd.Year - datStart.Year;

        public override string MouseAxisEvent(int idYear, bool action)
           => action
              ? ReportMain.P_formMain.NewPanel
                (
                   typeof(AxisTypeCourse), 
                   typeof(AxisTypeMonth), 
                   Globals.GetYearRange(P_reportDescriptor, idYear), 
                   idMember: P_reportDescriptor.P_idMember
                )
              : $"Klicken für Details zu " + HeaderStrings[0] + " " + GetDescription(idYear);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfYears(P_reportDescriptor.P_timeRange.P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idYear, int iNr = 1)
            => GetDesc(P_reportDescriptor, idYear, iNr);

        public static string GetDesc(ReportDescriptor desc, int idYear, int iNr = 1)
            => $"{desc.P_timeRange.P_datStart.Year + idYear}";
    }
}
