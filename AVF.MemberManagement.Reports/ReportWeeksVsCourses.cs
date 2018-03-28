using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeeksVsCourses : ReportTrainingsParticipation
    {
        public ReportWeeksVsCourses(TimeRange timeRange)
            : base(timeRange)
        {
            CreateModel
            (
                new AxisTypeCourse(timeRange),
                new AxisTypeWeek(timeRange),
                filter: tn => true
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(-1);   // -1: all members
            P_labelMonat.Text  = new AxisTypeMonth(timeRange).GetDescription(0);
            ReportFormPopulate();
        }
    }
}
