using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public bool m_activeColumnsOnly;

        protected HorizontalAxis(DatabaseWrapper db, TrainingParticipationReport coreReport)
            : base(db, coreReport)
        { }

        public abstract void FillHeaderRows(DataGridView dgv, int iNrOfColsOnLeftSide);

        public int GetNrOfDgvColumns()
            => m_activeColumnsOnly ? m_coreReport.GetNrOfActiveCols() : GetNrOfSrcElements();
        
        public void FillFooterRow(DataGridView dgv, int iNrOfColsOnLeftSide)
        {
            int iDgvRow = dgv.RowCount - 1;  // one footer row

            dgv[iNrOfColsOnLeftSide - 1, iDgvRow].Value = "Summe";

            int iDgvCol = iNrOfColsOnLeftSide;
            m_coreReport.ForAllColumns
            (
                action: iCol => dgv[iDgvCol++, iDgvRow].Value = m_coreReport.GetColSum(iCol),
                activeColumnsOnly: m_activeColumnsOnly
            );

            dgv[dgv.ColumnCount - 1, iDgvRow].Value = m_coreReport.GetSumSum();
        }
    }
}
