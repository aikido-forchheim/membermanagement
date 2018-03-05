using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining : ReportTrainingsParticipation
    {
        public ReportTraining(Training training)
            : base(training.Termin, training.Termin)
        {
            CreateModel
            (
                bHide: true,
                new AxisTypeEmpty(),
                new AxisTypeMember(),
                filter: tn => tn.TrainingID == training.Id
            );

            m_labelReportName.Text = $"Training am {Globals.DatabaseWrapper.WeekDay(training.WochentagID)} den ";
            m_labelZeitraum.Text   = new AxisTypeTraining().GetDescription(training.Id);
            m_Info0.Text           = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            m_Info1.Text           = $"Trainer: {m_axisTypeMember.GetFullDesc(training.Trainer)}";
            ReportFormPopulate();
        }
    }
}
