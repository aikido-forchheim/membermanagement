using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTrainingsVsCourses : ReportTrainingsParticipation
    {
        public ReportTrainingsVsCourses(TimeRange timeRange, int idMonth, int idMember)
            : base(timeRange, idMember, idMonth: idMonth)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeTraining(m_reportDescriptor)
            );

            ReportFormPopulate();
        }
    }
}
