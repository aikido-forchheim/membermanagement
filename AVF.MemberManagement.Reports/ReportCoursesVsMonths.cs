using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportCoursesVsMonths : ReportTrainingsParticipation
    {
        public ReportCoursesVsMonths(TimeRange timeRange, int idMember)
            : base(timeRange, idMember)
        {
            CreateModel
            (
                new AxisTypeMonth(m_reportDescriptor),
                new AxisTypeCourse(m_reportDescriptor)
            );

            ReportFormPopulate();
        }
    }
}
