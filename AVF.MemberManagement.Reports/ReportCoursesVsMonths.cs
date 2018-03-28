using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportCoursesVsMonths : ReportTrainingsParticipation
    {
        public ReportCoursesVsMonths(TimeRange timeRange, int idMember)
            : base(timeRange, idMember, Globals.ALL_COURSES)
        {
            CreateModel
            (
                new AxisTypeMonth(m_reportDescriptor),
                new AxisTypeCourse(m_reportDescriptor),
                filter: tn => (idMember == Globals.ALL_MEMBERS) ? true : (idMember == tn.MitgliedID)
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
