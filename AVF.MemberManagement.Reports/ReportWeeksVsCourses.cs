using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeeksVsCourses : ReportTrainingsParticipation
    {
        public ReportWeeksVsCourses(TimeRange timeRange, int idMember)
            : base(timeRange, idMember, Globals.ALL_COURSES)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeWeek(m_reportDescriptor),
                filter: tn => (idMember == Globals.ALL_MEMBERS) ? true : (idMember == tn.MitgliedID)
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(idMember); 
            P_labelMonat.Text  = new AxisTypeMonth(m_reportDescriptor).GetDescription(0);
            ReportFormPopulate();
        }
    }
}
