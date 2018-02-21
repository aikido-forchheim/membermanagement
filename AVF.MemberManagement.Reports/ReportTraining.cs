using System;
using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining : ReportForm
    {
        public ReportTraining(int idTraining)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);

            CreateModel
            (
                training.Termin, training.Termin,
                new HorizontalAxisTrainings(),
                new VerticalAxisMembers(),
                (tn => tn.TrainingID == idTraining)
            );

            string day = Globals.DatabaseWrapper.WeekDay(training.WochentagID);
            m_label1.Text = $"Training am {day} den {training.Termin:dd}.{training.Termin:MM}.{training.Termin:yyyy}";
            m_label2.Text = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
            m_label3.Text = $"Trainer: {Globals.GetMemberDescription(training.Trainer)}";
        }

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            return String.Empty;
        }
    }
}
