using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourses : ReportTrainingsParticipation
    {
        public ReportMemberVsCourses( TimeRange timeRange, int idMember )
            : base(timeRange, idMember, Globals.ALL_COURSES, Globals.ALL_TRAININGS)
        {
            CreateModel
            (
                new AxisTypeCourse(m_reportDescriptor),
                new AxisTypeMember(m_reportDescriptor)
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(Globals.ALL_MEMBERS);
            ReportFormPopulate();
        }

        protected override string MouseMainDataAreaCellEvent(TimeRange timeRange, int idMember, int idKurs, bool action )
            => action
                ? ReportMain.P_formMain.SwitchToPanel( new ReportMemberVsTrainings(timeRange, idMember, idKurs) )
                : $"Klicken für Details zur Teilnahme von\n"
                     + P_axisTypeMember.GetFullDesc(idMember)
                     + $" am Kurs\n" 
                     + P_xAxisType.GetDescription(idKurs);
    }
}
