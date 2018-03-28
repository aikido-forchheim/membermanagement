using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourses : ReportTrainingsParticipation
    {
        public ReportMemberVsCourses( TimeRange timeRange )
            : base(timeRange)
        {
            CreateModel
            (
                new AxisTypeCourse(timeRange),
                new AxisTypeMember(timeRange),
                filter: tn => true
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(-1);   // -1: all members
            ReportFormPopulate();
        }

        protected override string MouseMainDataAreaCellEvent(TimeRange timeRange, int idMember, int idKurs, bool action )
            => action
                ? ReportMain.P_formMain.SwitchToPanel( new ReportMemberVsTrainings(timeRange, idKurs) )
                : $"Klicken für Details zur Teilnahme von\n"
                     + P_axisTypeMember.GetFullDesc(idMember)
                     + $" am Kurs\n" 
                     + P_xAxisType.GetDescription(idKurs);
    }
}
