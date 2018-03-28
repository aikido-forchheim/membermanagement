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

            P_labelReportName.Text = $"Training am {Globals.DatabaseWrapper.WeekDay(training.WochentagID)} den ";
            P_labelZeitraum.Text   = new AxisTypeTraining(m_reportDescriptor).GetFullDesc(training.Id, '.');
//            P_labelMember.Text     = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            ReportFormPopulate();
        }
    }
}
