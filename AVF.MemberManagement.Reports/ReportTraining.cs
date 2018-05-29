using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining
    {
        public ReportTraining(Training training)
        {
            ReportMain.P_formMain.NewTrainingsParticipationPanel
            (
                defaultDesc: null,
                xAxisType: typeof(AxisTypeEmpty), 
                yAxisType: typeof(AxisTypeMember), 
                timeRange: new TimeRange(training.Termin), 
                idTraining: training.Id
            );
        }
    }
}
