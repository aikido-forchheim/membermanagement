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
                new AxisTypeEmpty(P_datStart, P_datEnd),
                new AxisTypeMember(P_datStart, P_datEnd),
                filter: tn => tn.TrainingID == training.Id
            );

            P_labelReportName.Text = $"Training am {Globals.DatabaseWrapper.WeekDay(training.WochentagID)} den ";
            P_labelZeitraum.Text   = new AxisTypeTraining(P_datStart, P_datEnd).GetFullDesc(training.Id, '.');
            P_Info0.Text           = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            P_Info1.Text           = $"Trainer: {P_axisTypeMember.GetFullDesc(training.Trainer)}";
            ReportFormPopulate();
        }
    }
}
