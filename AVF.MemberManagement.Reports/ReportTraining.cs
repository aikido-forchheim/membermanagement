using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining : ReportTrainingsParticipation
    {
        public ReportTraining(Training training)
            : base(new TimeRange(training.Termin), Globals.ALL_MEMBERS, Globals.ALL_COURSES, training.Id)
        {
            CreateModel
            (
                new AxisTypeEmpty(m_reportDescriptor),
                new AxisTypeMember(m_reportDescriptor)
            );

            P_labelReportName.Text = $"Training am {Globals.DatabaseWrapper.WeekDay(training.WochentagID)} den ";
            P_labelZeitraum.Text   = new AxisTypeTraining(m_reportDescriptor).GetFullDesc(training.Id, '.');
            P_labelMember.Text     = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            P_labelTrainer.Text    = $"Trainer: {P_axisTypeMember.GetFullDesc(training.Trainer)}";
            ReportFormPopulate();
        }
    }
}
