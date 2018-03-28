using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMonthsVsCourses : ReportTrainingsParticipation
    {
        public ReportMonthsVsCourses(TimeRange timeRange, int idMember)
            : base(timeRange, idMember, Globals.ALL_COURSES, Globals.ALL_TRAININGS)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeMonth(m_reportDescriptor)
            );

            P_labelMember.Text  = P_axisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
