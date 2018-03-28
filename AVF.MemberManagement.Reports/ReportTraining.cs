using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining : ReportTrainingsParticipation
    {
        public ReportTraining(Training training)
            : base(new TimeRange(training.Termin))
        {
            CreateModel
            (
                new AxisTypeEmpty(new TimeRange(training.Termin)),
                new AxisTypeMember(new TimeRange(training.Termin)),
                filter: tn => tn.TrainingID == training.Id
            );

            P_labelReportName.Text = $"Training am {Globals.DatabaseWrapper.WeekDay(training.WochentagID)} den ";
            P_labelZeitraum.Text   = new AxisTypeTraining(P_timeRange).GetFullDesc(training.Id, '.');
            P_labelMember.Text     = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            P_labelTrainer.Text    = $"Trainer: {P_axisTypeMember.GetFullDesc(training.Trainer)}";
            ReportFormPopulate();
        }
    }
}
