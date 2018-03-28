using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMonthsVsCourses : ReportTrainingsParticipation
    {
        public ReportMonthsVsCourses(TimeRange timeRange, int idMember)
            : base(timeRange)
        {
            CreateModel
            (
                new AxisTypeCourse(timeRange),
                new AxisTypeMonth(timeRange),
                filter: tn => idMember == tn.MitgliedID
            );

            P_labelMember.Text  = P_axisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
