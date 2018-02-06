using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        public int m_iNrOfColsOnLeftSide;

        public bool m_activeRowsOnly;

        protected VerticalAxis
        (
            DatabaseWrapper db,
            TrainingParticipationReport coreReport
        )
            : base(db, coreReport)
        { }

        abstract public int GetNrOfSrcRows();
        abstract public int GetColumnIndexFromTrainingsParticipation(TrainingsTeilnahme tn );
        abstract public void FillRowHeaderColumns(DataGridView dgv);
        abstract public void FillRowSumColumns(DataGridView dgv);

        public int GetNrOfDgvRows()
            => m_activeRowsOnly ? m_coreReport.GetNrOfActiveRows() : GetNrOfSrcRows();
        
        protected void FillRowSumColumns(DataGridView dgv, bool activeRowsOnly)
        {
            int iDgvRow = 0;
            m_coreReport.ForAllRows
            (
                iRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = m_coreReport.GetRowSum(iRow),
                activeRowsOnly
            );
        }
    }
}
