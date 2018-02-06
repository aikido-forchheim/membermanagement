using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportMemberVsTraining: ReportBase
    {
        private int m_idKurs;

        public ReportMemberVsTraining(DatabaseWrapper db, DateTime datStart, DateTime datEnd, int idKurs)
            : base(db, datStart, datEnd )
        {
            m_idKurs = idKurs;
            m_iNrOfColsOnLeftSide = 3;   // 3 columns for Mitglieder

            m_xAxis = new HorizontalAxisTrainings(db, m_coreReport, datStart, datEnd, idKurs );
            m_yAxis = new VerticalAxisMembers(db, m_coreReport);

            m_coreReport.Initialize
            (
                m_yAxis.GetNrOfSrcRows( ),
                m_xAxis.GetNrOfSrcColumns(),
                tn => m_db.KursIdFromTrainingId(tn.TrainingID) == m_idKurs,
                tn => m_yAxis.GetColumnIndexFromTrainingsParticipation(tn),
                tn => m_xAxis.GetRowIndexFromTrainingsParticipation(tn)
            );

            m_coreReport.SortRows();
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue == 1) ? " X " : "   ";
        }
    }
}
