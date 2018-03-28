using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportCoursesVsMonths : ReportTrainingsParticipation
    {
        public ReportCoursesVsMonths(TimeRange timeRange, int idMember)
            : base(timeRange, idMember, Globals.ALL_COURSES, Globals.ALL_TRAININGS)
        {
            CreateModel
            (
                new AxisTypeMonth(m_reportDescriptor),
                new AxisTypeCourse(m_reportDescriptor)
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
