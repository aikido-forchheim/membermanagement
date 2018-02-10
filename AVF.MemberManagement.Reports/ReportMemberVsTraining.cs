using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTraining : ReportForm
    {
        public ReportMemberVsTraining(DateTime datStart, DateTime datEnd, int idKurs)
            : base(datStart, datEnd)
        {
            m_xAxis = new HorizontalAxisTrainings(datStart, datEnd, idKurs);
            m_yAxis = new VerticalAxisMembers();

            m_tpModel = new TrainingParticipationModel
            (
                datStart,
                datEnd,
                m_xAxis,
                m_yAxis,
                (tn => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID) == idKurs)
            );
        }
    }
}
