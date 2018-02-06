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
                m_xAxis,
                m_yAxis,
                tn => m_db.KursIdFromTrainingId(tn.TrainingID) == m_idKurs
            );

            m_coreReport.SortRows();
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue == 1) ? " X " : "   ";
        }
    }
}
