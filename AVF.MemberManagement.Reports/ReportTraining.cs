using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining : ReportTrainingsParticipation
    {
        public ReportTraining(Training training)
            : base(timeRange: new TimeRange(training.Termin), idTraining: training.Id)
        {
            CreateModel
            (
                new AxisTypeEmpty(m_reportDescriptor),
                new AxisTypeMember(m_reportDescriptor)
            );

            ReportFormPopulate();
        }
    }
}
