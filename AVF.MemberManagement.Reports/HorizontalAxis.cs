using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public int m_iNrOfFooterRows { get; protected set; }

        public bool m_activeColsOnly;

        protected HorizontalAxis(DatabaseWrapper db, TrainingParticipationMatrix tpMatrix)
            : base(db, tpMatrix)
        {
            m_iNrOfFooterRows = 1; // 1 row for column sum
        }

        public abstract void FillHeaderRows(DataGridView dgv, int iNrOfColsOnLeftSide);

        public int GetNrOfDgvCols()
            => m_activeColsOnly ? m_tpMatrix.GetNrOfActiveCols() : GetNrOfSrcElements();
        
        public void FillFooterRow(DataGridView dgv, int iNrOfColsOnLeftSide)
        {
            int iDgvRow = dgv.RowCount - 1;  // one footer row

            dgv[iNrOfColsOnLeftSide - 1, iDgvRow].Value = "Summe";

            int iDgvCol = iNrOfColsOnLeftSide;
            m_tpMatrix.ForAllCols
            (
                action: iCol => dgv[iDgvCol++, iDgvRow].Value = m_tpMatrix.GetColSum(iCol),
                activeColsOnly: m_activeColsOnly
            );

            dgv[dgv.ColumnCount - 1, iDgvRow].Value = m_tpMatrix.GetSumSum();
        }
    }
}
