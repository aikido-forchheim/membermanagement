using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsMonths : ReportTrainingsParticipation
    {
        public ReportMemberVsMonths(TimeRange timeRange, int idMember, int idCourse)
            : base(timeRange, idMember, idCourse, Globals.ALL_TRAININGS)
        {
            CreateModel
            (
                new AxisTypeMonth(m_reportDescriptor),
                new AxisTypeMember(m_reportDescriptor)
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(Globals.ALL_MEMBERS);   
            ReportFormPopulate();
        }

        protected override string MouseMainDataAreaCellEvent(TimeRange timeRange, int idMember, int idMonth, bool action)
            => action
                ? ReportMain.P_formMain.SwitchToPanel(new ReportTrainingsVsCourses(timeRange, idMonth, idMember))
                : $"Klicken für Details zur Teilnahme von\n"
                     + P_axisTypeMember.GetFullDesc(idMember)
                     + $" im Monat\n"
                     + P_xAxisType.GetDescription(idMonth);
    }
}
