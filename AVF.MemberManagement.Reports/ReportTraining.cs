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

            string day = Globals.DatabaseWrapper.WeekDay(training.WochentagID);
            m_labelReportName.Text = $"Training am {day} den ";
            m_labelZeitraum.Text   = AxisTypeTraining.GetDesc(training.Id, ' ');
            m_Info0.Text           = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            m_Info1.Text           = $"Trainer: {AxisTypeMember.GetDesc(training.Trainer, ' ')}";
            ReportFormPopulate();
        }
    }
}
