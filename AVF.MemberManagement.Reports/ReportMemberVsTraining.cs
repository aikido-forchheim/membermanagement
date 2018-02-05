using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportMemberVsTraining: ReportBase
    {
        private int m_idKurs;

        public ReportMemberVsTraining(DatabaseWrapper db, DateTime datStart, DateTime datEnd, int idKurs)
            : base(db, datStart, datEnd)
        {
            m_idKurs = idKurs;
            m_iNrOfColsOnLeftSide  = 3;   // 3 columns for Mitglieder
            m_iNrOfColsOnRightSide = 1;   // 1 column for row sum

            m_coreReport.Initialize
            (
                datStart, 
                datEnd,
                m_db.MaxMitgliedsNr() + 1,   // One additional row for pseudo member with Id = -1
                m_db.MaxTrainingNr() + 1,
                tn => m_db.KursIdFromTrainingId(tn.TrainingID) == m_idKurs,
                tn => tn.MitgliedID - 1,    // db ids start with 1, array indeices with 0
                tn => tn.TrainingID - 1     // db ids start with 1, array indeices with 0
            );

            m_coreReport.SortRows();
        }

        protected override void SizeDataGridView(DataGridView dgv)
        {
            dgv.RowCount    = m_coreReport.GetNrOfActiveRows() + 1;  // one footer row
            dgv.ColumnCount = m_coreReport.GetNrOfActiveCols() + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;
        }

        protected override void FillHeaderRows(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Monat\nTag";
            int iCol = m_iNrOfColsOnLeftSide;
            foreach (var training in m_db.TrainingsInPeriod(m_idKurs, m_datStart, m_datEnd))
            {
                dgv.Columns[iCol++].HeaderText = $"{training.Termin:MM}\n{training.Termin:dd}";
            }
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
        }

        protected override void FillRowHeaderColumns(DataGridView dgv)
        {
            FillMemberRowHeaderColumns(dgv);
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue == 1) ? " X " : "   ";
        }
    }
}
