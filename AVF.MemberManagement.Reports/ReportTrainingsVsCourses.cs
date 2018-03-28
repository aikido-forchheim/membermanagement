using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTrainingsVsCourses : ReportTrainingsParticipation
    {
        public ReportTrainingsVsCourses(TimeRange timeRange, int idMonth)
            : base(timeRange)
        {
            CreateModel
            (
                new AxisTypeCourse(timeRange),
                new AxisTypeTraining(timeRange),
                filter: tn => true
            );

            P_labelMonat.Text = "Monat:" + new AxisTypeMonth(timeRange).GetDescription(idMonth);
            ReportFormPopulate();
        }
    }
}
