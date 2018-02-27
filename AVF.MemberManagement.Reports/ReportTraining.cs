using System.Diagnostics;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining : ReportTrainingsParticipation
    {
        public ReportTraining(int idTraining)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);

            CreateModel
            (
                training.Termin, training.Termin,
                new HorizontalAxisEmpty(),
                new VerticalAxisMembers(),
                filter: tn => tn.TrainingID == idTraining
            );

            string day = Globals.DatabaseWrapper.WeekDay(training.WochentagID);
            m_label1.Text = $"Training am {day} den {training.Termin:dd}.{training.Termin:MM}.{training.Termin:yyyy}";
            m_label2.Text = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            m_label3.Text = $"Trainer: {m_yAxis.P_AxisType.GetDescription(training.Trainer)}";
        }
    }
}
