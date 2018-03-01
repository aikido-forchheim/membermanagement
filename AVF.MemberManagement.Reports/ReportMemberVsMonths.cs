using System;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsMonths : ReportTrainingsParticipation
    {
        public ReportMemberVsMonths(DateTime datStart, DateTime datEnd)
        {
            CreateModel
            (
                bHide: false,
                datStart, datEnd,
                new AxisTypeMonth(datStart, datEnd),
                new AxisTypeMember(),
                filter: tn => true
            );

            m_labelReportName.Text = "Überblick Trainingsteilnahme";
        }
    }
}
