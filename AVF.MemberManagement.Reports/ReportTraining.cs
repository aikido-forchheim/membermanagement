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
                bHide: true,
                training.Termin, training.Termin,
                new AxisTypeEmpty(),
                new AxisTypeMember(),
                new VerticalAxisMembers(),
                filter: tn => tn.TrainingID == idTraining
            );

            string day = Globals.DatabaseWrapper.WeekDay(training.WochentagID);
            m_labelReportName.Text = $"Training am {day} den ";
            m_labelZeitraum.Text   = AxisTypeTraining.GetDesc(idTraining, ' ');
            m_Info0.Text           = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            m_Info1.Text           = $"Trainer: {AxisTypeMember.GetDesc(training.Trainer, ' ')}";
        }
    }
}
