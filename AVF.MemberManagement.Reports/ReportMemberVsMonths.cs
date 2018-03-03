using System;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsMonths : ReportTrainingsParticipation
    {
        public ReportMemberVsMonths(DateTime datStart, DateTime datEnd)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeMonth(datStart, datEnd),
                new AxisTypeMember(),
                filter: tn => true
            );

            m_labelReportName.Text = AxisTypeCourse.Title;
            ReportFormPopulate();
        }
    }
}
