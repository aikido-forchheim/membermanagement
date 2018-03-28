using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTrainingsVsCourses : ReportTrainingsParticipation
    {
        public ReportTrainingsVsCourses(TimeRange timeRange, int idMonth, int idMember)
            : base(timeRange, idMember, Globals.ALL_COURSES, Globals.ALL_TRAININGS)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeTraining(m_reportDescriptor)
            );

            P_labelMonat.Text = "Monat:" + new AxisTypeMonth(m_reportDescriptor).GetDescription(idMonth);
            ReportFormPopulate();
        }
    }
}
