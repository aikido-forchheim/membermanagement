using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class ReportBase
    {
        protected DatabaseWrapper m_db;

        protected DateTime m_datStart;
        protected DateTime m_datEnd;

        public int m_iNrOfColsOnLeftSide  { get; protected set; }
        public int m_iNrOfColsOnRightSide { get; protected set; }

        protected TrainingParticipationReport m_coreReport;

        protected VerticalAxis   m_yAxis;
        protected HorizontalAxis m_xAxis;

        public ReportBase
        (
            DatabaseWrapper db, 
            DateTime datStart, 
            DateTime datEnd
        )
        {
            m_db = db;
            m_datStart = datStart;
            m_datEnd = datEnd;
            m_coreReport = new TrainingParticipationReport(db, datStart, datEnd);
        }

        protected void FillMainRows(DataGridView dgv)
        {
            int iDgvRow = 0;
            dgv.Rows[dgv.RowCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            m_coreReport.ForAllRows
            (
                iRow =>
                {
                    int iDgvCol = m_iNrOfColsOnLeftSide;
                    m_coreReport.ForAllColumns
                    (
                        iCol => dgv[iDgvCol++, iDgvRow].Value = FormatMatrixElement( m_coreReport.GetCell(iRow, iCol) ),
                        m_xAxis.m_activeColumnsOnly
                    );
                    ++iDgvRow;
                },
                m_yAxis.m_activeRowsOnly
            );
        }

        public void PopulateGridView(DataGridView dgv)
        {
            dgv.RowCount    = m_yAxis.GetNrOfDgvRows() + 1;  // one footer row
            dgv.ColumnCount = m_xAxis.GetNrOfDgvColumns() + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;

            m_xAxis.FillHeaderRows( dgv, m_yAxis.m_iNrOfColsOnLeftSide );
            m_xAxis.FillFooterRow ( dgv, m_yAxis.m_iNrOfColsOnLeftSide );
            m_yAxis.FillRowHeaderColumns( dgv );
            m_yAxis.FillRowSumColumns   ( dgv );
            FillMainRows( dgv );
        }

        protected abstract string FormatMatrixElement(int iValue);
    }
}
