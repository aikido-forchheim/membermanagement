using System;
using System.Linq;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
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

        public ReportBase(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
        {
            m_db = db;
            m_datStart = datStart;
            m_datEnd = datEnd;
            m_coreReport = new TrainingParticipationReport(db);
        }

        protected void FillCourseHeaderRows(DataGridView dgv)
        {
            int iDgvCol = m_iNrOfColsOnLeftSide;

            dgv.Columns[iDgvCol].HeaderText = "Lehrg.\netc.";
            dgv.Columns[iDgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            iDgvCol++;
            m_coreReport.ForAllColumns
            (
                action: iCol =>
                {
                    dgv.Columns[iDgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (iCol > 0)
                    {
                        Kurs kurs = m_db.KursFromId(iCol);
                        dgv.Columns[iDgvCol++].HeaderText = $"{ m_db.WeekDay(kurs.WochentagID).Substring(0, 2) }\n{kurs.Zeit:hh}:{kurs.Zeit:mm}";
                    }
                },
                activeColumnsOnly: false
            );
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
            dgv.Columns[dgv.ColumnCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        protected void FillMemberRowHeaderColumns(DataGridView dgv)
        {
            int iDgvRow = 0;
            dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            m_coreReport.ForAllRows
            (
                action: iRow =>
                {
                    Mitglied mitglied = m_db.MitgliedFromId(m_coreReport.GetRowId(iRow));
                    dgv[0, iDgvRow].Value = mitglied.Nachname;
                    dgv[1, iDgvRow].Value = mitglied.Vorname;
                    dgv[2, iDgvRow].Value = mitglied.Id;
                    ++iDgvRow;
                },
                activeRowsOnly: true
            );
        }

        protected void FillMainRows(DataGridView dgv, bool activeCellsOnly)
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
                        activeCellsOnly
                    );
                    ++iDgvRow;
                },
                activeCellsOnly
            );
        }

        protected void FillRowSumColumn(DataGridView dgv, bool activeRowsOnly)
        {
            int iDgvRow = 0;
            m_coreReport.ForAllRows
            (
                iRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = m_coreReport.GetRowSum(iRow),
                activeRowsOnly
            );
        }

        protected void FillFooterRow(DataGridView dgv )
        {
            int iDgvRow = dgv.RowCount - 1;  // one footer row

            dgv[m_iNrOfColsOnLeftSide-1, iDgvRow].Value = "Summe";

            int iDgvCol = m_iNrOfColsOnLeftSide;
            m_coreReport.ForAllColumns
            (
                action: iCol => dgv[iDgvCol++, iDgvRow].Value = m_coreReport.GetColSum(iCol),
                activeColumnsOnly: true
            );

            dgv[dgv.ColumnCount - 1, iDgvRow].Value = m_coreReport.GetSumSum();
        }

        public void PopulateGridView(DataGridView dgv, bool activeCellsOnly)
        {
            SizeDataGridView(dgv);
            FillHeaderRows(dgv);
            FillRowHeaderColumns(dgv);
            FillMainRows(dgv, activeCellsOnly);
            FillRowSumColumn(dgv, activeCellsOnly);
            FillFooterRow(dgv);
        }
        abstract protected void SizeDataGridView(DataGridView dgv);
        abstract protected void FillRowHeaderColumns(DataGridView dgv);
        abstract protected void FillHeaderRows(DataGridView dgv);
        abstract protected string FormatMatrixElement(int iValue);
    }
}
