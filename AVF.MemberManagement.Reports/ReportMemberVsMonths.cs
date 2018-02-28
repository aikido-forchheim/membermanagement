using System;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsMonths : ReportTrainingsParticipation
    {
        public ReportMemberVsMonths(DateTime datStart, DateTime datEnd)
        {
            CreateModel
            (
                datStart, datEnd,
                new AxisTypeMonth(datStart, datEnd),
                new AxisTypeMember(),
                new HorizontalAxisMonths(datStart, datEnd),
                new VerticalAxisMembers(),
                filter: tn => true
            );

            m_labelReportName.Text = "Überblick Trainingsteilnahme";
        }
    }
}
