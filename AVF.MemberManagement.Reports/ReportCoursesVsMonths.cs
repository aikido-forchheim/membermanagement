using System;

namespace AVF.MemberManagement.Reports
{
    class ReportCoursesVsMonths : ReportTrainingsParticipation
    {
        public ReportCoursesVsMonths(DateTime datStart, DateTime datEnd, int idMember)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeMonth(datStart, datEnd),
                new AxisTypeCourse(),
                filter: tn => (idMember == -1) ? true : (idMember == tn.MitgliedID)
            );

            if (idMember != -1)
                m_Info0.Text = AxisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
