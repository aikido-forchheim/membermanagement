using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMonthsVsCourses : ReportTrainingsParticipation
    {
        public ReportMonthsVsCourses(TimeRange timeRange, int idMember)
            : base(timeRange, idMember, Globals.ALL_COURSES)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeMonth(m_reportDescriptor),
                filter: tn => (idMember == Globals.ALL_MEMBERS) ? true : (idMember == tn.MitgliedID)
            );

            P_labelMember.Text  = P_axisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
