using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeeksVsCourses : ReportTrainingsParticipation
    {
        public ReportWeeksVsCourses(TimeRange timeRange, int idMember)
            : base(timeRange, idMember)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeWeek(m_reportDescriptor)
            );

            ReportFormPopulate();
        }
    }
}
