using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining : ReportTrainingsParticipation
    {
        public ReportTraining(Training training)
            : base(typeof(AxisTypeEmpty), typeof(AxisTypeMember), timeRange: new TimeRange(training.Termin), idTraining: training.Id)
        {
        }
    }
}
