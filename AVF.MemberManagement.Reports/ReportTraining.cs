using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportTraining : ReportForm
    {
        public ReportTraining( int idTraining )
        {
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
 
            m_xAxis = new HorizontalAxisTrainings();
            m_yAxis = new VerticalAxisMembers();

            m_tpModel = new TrainingParticipationModel
            (
                training.Termin,
                training.Termin,
                m_xAxis,
                m_yAxis,
                (tn => tn.TrainingID == idTraining)
            );

            string day = Globals.DatabaseWrapper.WeekDay(training.WochentagID);
            m_label1.Text = $"Training am {day} den {training.Termin:dd}.{training.Termin:MM}.{training.Termin:yyyy}"; 
            m_label2.Text = $"um {training.Zeit:hh}:{training.Zeit:mm} Uhr";
        }
    }
}
