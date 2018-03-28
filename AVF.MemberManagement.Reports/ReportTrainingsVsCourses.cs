using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTrainingsVsCourses : ReportTrainingsParticipation
    {
        public ReportTrainingsVsCourses(TimeRange timeRange, int idMonth, int idMember)
            : base(timeRange, idMember, Globals.ALL_COURSES)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeTraining(m_reportDescriptor),
                filter: tn => true
            );

            P_labelMonat.Text = "Monat:" + new AxisTypeMonth(m_reportDescriptor).GetDescription(idMonth);
            ReportFormPopulate();
        }
    }
}
