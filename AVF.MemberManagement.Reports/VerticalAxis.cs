using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        public int m_iNrOfColsOnLeftSide  { get; protected set; }
        public int m_iNrOfColsOnRightSide { get; protected set; }

        public bool m_activeRowsOnly;

        protected VerticalAxis
        (
            DatabaseWrapper db,
            TrainingParticipationMatrix tpMatrix
        )
            : base(db, tpMatrix)
        {
            m_iNrOfColsOnRightSide = 1;  // 1 column for row sum
        }

        public abstract void FillRowHeaderCols(DataGridView dgv);
        public abstract void FillRowSumCols(DataGridView dgv);

        public int GetNrOfDgvRows()
            => m_activeRowsOnly ? m_tpMatrix.GetNrOfActiveRows() : GetNrOfSrcElements();
        
        protected void FillRowSumCols(DataGridView dgv, bool activeRowsOnly)
        {
            int iDgvRow = 0;
            m_tpMatrix.ForAllRows
            (
                iRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = m_tpMatrix.GetRowSum(iRow),
                activeRowsOnly
            );
        }
    }
}
