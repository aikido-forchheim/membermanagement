using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportCoursesVsMonths : ReportTrainingsParticipation
    {
        public ReportCoursesVsMonths(TimeRange timeRange, int idMember)
            : base(timeRange)
        {
            CreateModel
            (
                new AxisTypeMonth(timeRange),
                new AxisTypeCourse(timeRange),
                filter: tn => (idMember == -1) ? true : (idMember == tn.MitgliedID)
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
