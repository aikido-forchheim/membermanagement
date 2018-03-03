using System;

namespace AVF.MemberManagement.Reports
{
    class ReportMonthsVsCourses : ReportTrainingsParticipation
    {
        public ReportMonthsVsCourses(DateTime datStart, DateTime datEnd, int idMember)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeCourse(),
                new AxisTypeMonth(datStart, datEnd),
                filter: tn => idMember == tn.MitgliedID
            );

            m_labelReportName.Text = AxisTypeMember.Title;
            m_Info0.Text  = AxisTypeMember.GetDesc(idMember, ' ');
            ReportFormPopulate();
        }
    }
}
