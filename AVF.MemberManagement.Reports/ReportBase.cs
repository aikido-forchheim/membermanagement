using System;
using System.Collections;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public enum ReportType { MemberVsTraining, MemberVsCourse, WeekVsCourse };

    public class ReportDescriptor
    {
        public ReportDescriptor(ReportType type, int param = 0)
        {
            m_type = type;
            m_param = param;
        }

        public ReportType m_type { get; set; }
        public int        m_param { get; set; }
    }

    public abstract class ReportBase
    {
        protected DatabaseWrapper m_db;

        protected DateTime m_datStart;
        protected DateTime m_datEnd;

        protected TrainingParticipationMatrix m_tpMatrix;

        protected VerticalAxis   m_yAxis;
        protected HorizontalAxis m_xAxis;

        private DataGridView m_dataGridView;

        public class ReportDescription
        {
            public ReportDescription(int? idMember, int? idCourse, int? idWeek)
            {
                m_idMember = idMember;
                m_idCourse = idCourse;
                m_idWeek = idWeek;
            }

            public int? m_idMember { get; set; }
            public int? m_idCourse { get; set; }
            public int? m_idWeek   { get; set; }
        }

        public void ShowReport(ReportDescriptor desc)
        {
            ReportBase m_report;

            switch (desc.m_type)
            {
                case ReportType.MemberVsTraining:
                    m_report = new ReportMemberVsTraining(m_db, m_datStart, m_datEnd, desc.m_param);
                    m_report.PopulateGridView(m_dataGridView);
                    break;
                case ReportType.MemberVsCourse:
                    m_report = new ReportMemberVsCourse(m_db, m_datStart, m_datEnd);
                    m_report.PopulateGridView(m_dataGridView);
                    break;
                case ReportType.WeekVsCourse:
                    m_report = new ReportWeekVsCourse(m_db, m_datStart, m_datEnd, desc.m_param);
                    m_report.PopulateGridView(m_dataGridView);
                    break;
            }
        }

        public ReportBase
        (
            DatabaseWrapper db, 
            DateTime datStart, 
            DateTime datEnd
        )
        {
            m_db         = db;
            m_datStart   = datStart;
            m_datEnd     = datEnd;
            m_tpMatrix = new TrainingParticipationMatrix(db, datStart, datEnd);
        }

        protected void FillMainRows(DataGridView dgv)
        {
            int iDgvRow = 0;
            dgv.Rows[dgv.RowCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            m_tpMatrix.ForAllRows
            (
                iRow =>
                {
                    int iDgvCol = m_yAxis.m_iNrOfColsOnLeftSide;
                    m_tpMatrix.ForAllCols
                    (
                        iCol => dgv[iDgvCol++, iDgvRow].Value = FormatMatrixElement( m_tpMatrix.GetCell(iRow, iCol) ),
                        m_xAxis.m_activeColsOnly
                    );
                    ++iDgvRow;
                },
                m_yAxis.m_activeRowsOnly
            );
        }

        public void PopulateGridView(DataGridView dgv)
        {
            dgv.RowCount    = m_yAxis.GetNrOfDgvRows() + m_xAxis.m_iNrOfFooterRows;  // one footer row
            dgv.ColumnCount = m_xAxis.GetNrOfDgvCols() + m_yAxis.m_iNrOfColsOnLeftSide + m_yAxis.m_iNrOfColsOnRightSide;

            m_xAxis.FillHeaderRows( dgv, m_yAxis.m_iNrOfColsOnLeftSide );
            m_xAxis.FillFooterRow ( dgv, m_yAxis.m_iNrOfColsOnLeftSide );
            m_yAxis.FillRowHeaderCols( dgv );
            m_yAxis.FillRowSumCols   ( dgv );
            FillMainRows( dgv );
            m_dataGridView = dgv;
            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
        }

        private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            bool isOnMainAreaRow =                             (0  < e.RowIndex)    && (e.RowIndex    < m_dataGridView.RowCount);
            bool IsOnMainAreaCol = (m_yAxis.m_iNrOfColsOnLeftSide <= e.ColumnIndex) && (e.ColumnIndex < m_dataGridView.ColumnCount);

            if (isOnMainAreaRow) 
            {
                if (IsOnMainAreaCol)
                {
                    int idKurs = (e.ColumnIndex == m_yAxis.m_iNrOfColsOnLeftSide) ? 0 : e.ColumnIndex - m_yAxis.m_iNrOfColsOnLeftSide;
                    ShowReport(new ReportDescriptor(ReportType.MemberVsTraining, idKurs) );
                }
                else // row header or row sum
                {
                    int idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                    ShowReport(new ReportDescriptor(ReportType.WeekVsCourse, idMember) );
                }
            }
            else  // header or footer 
            {
                if (IsOnMainAreaCol)
                {
                    int idKurs = e.ColumnIndex - m_yAxis.m_iNrOfColsOnLeftSide;
                    ShowReport( new ReportDescriptor(ReportType.MemberVsTraining, idKurs) );
                }
            }
        }

        protected abstract string FormatMatrixElement(int iValue);
    }
}
