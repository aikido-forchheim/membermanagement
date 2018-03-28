using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMonthsVsCourses : ReportTrainingsParticipation
    {
        public ReportMonthsVsCourses(TimeRange timeRange, int idMember)
            : base(timeRange, idMember)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeMonth(m_reportDescriptor)
            );

            ReportFormPopulate();
        }
    }
}
