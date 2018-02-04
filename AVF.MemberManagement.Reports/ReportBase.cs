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

        protected int m_iNrOfColsOnLeftSide;
        protected int m_iNrOfColsOnRightSide;

        protected TrainingParticipationReport m_coreReport;

        public ReportBase(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
        {
            m_db = db;
            m_datStart = datStart;
            m_datEnd = datEnd;
            m_coreReport = new TrainingParticipationReport(db);
        }

        protected int GetNrOfDgvRows()
            => m_coreReport.GetNrOfActiveRows() + 1;  // one footer row

        protected int GetNrOfDgvCols()
            => m_coreReport.GetNrOfActiveCols() + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;

        protected void FillCourseHeaderRows(DataGridView dgv)
        {
            int iDgvCol = m_iNrOfColsOnLeftSide;

            dgv.Columns[iDgvCol].HeaderText = "Lehrg.\netc.";
            dgv.Columns[iDgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            iDgvCol++;
            m_coreReport.ForAllActiveColumns
            (
                iCol =>
                {
                    dgv.Columns[iDgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (iCol > 0)
                    {
                        Kurs kurs = m_db.KursFromId(iCol);
                        dgv.Columns[iDgvCol++].HeaderText = $"{ m_db.WeekDay(kurs.WochentagID).Substring(0, 2) }\n{kurs.Zeit:hh}:{kurs.Zeit:mm}";
                    }
                }
            );
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
            dgv.Columns[dgv.ColumnCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        protected void FillMemberRowHeaderColumns(DataGridView dgv)
        {
            int iDgvRow = 0;
            dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            m_coreReport.ForAllActiveRows
            (
                iRow =>
                {
                    Mitglied mitglied = m_db.MitgliedFromId(m_coreReport.GetRowId(iRow));
                    dgv[0, iDgvRow].Value = mitglied.Nachname;
                    dgv[1, iDgvRow].Value = mitglied.Vorname;
                    dgv[2, iDgvRow].Value = mitglied.Id;
                    ++iDgvRow;
                }
            );
        }

        protected void FillMainRows(DataGridView dgv)
        {
            int iDgvRow = 0;
            dgv.Rows[dgv.RowCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            m_coreReport.ForAllActiveRows
            (
                iRow =>
                {
                    int iDgvCol = m_iNrOfColsOnLeftSide;
                    m_coreReport.ForAllActiveColumns
                    (
                        iCol => dgv[iDgvCol++, iDgvRow].Value = FormatMatrixElement( m_coreReport.GetCell(iRow, iCol) )
                    );
                    ++iDgvRow;
                }
            );
        }

        protected void FillRowSumColumn(DataGridView dgv)
        {
            int iDgvRow = 0;
            m_coreReport.ForAllActiveRows
            (
                iRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = m_coreReport.GetRowSum(iRow)
            );
        }

        protected void FillFooterRow(DataGridView dgv )
        {
            int iDgvRow = dgv.RowCount - 1;  // one footer row

  //          dgv[m_iNrOfColsOnLeftSide-1, iDgvRow].Value = "Summe";

            int iDgvCol = m_iNrOfColsOnLeftSide;
            m_coreReport.ForAllActiveColumns
            (
                iCol => dgv[iDgvCol++, iDgvRow].Value = m_coreReport.GetColSum(iCol)
            );

            dgv[dgv.ColumnCount - 1, iDgvRow].Value = m_coreReport.GetSumSum();
        }

        public void PopulateGridView(DataGridView dgv)
        {
            dgv.RowCount = GetNrOfDgvRows();  // one footer row
            dgv.ColumnCount = GetNrOfDgvCols();

            FillHeaderRows(dgv);
            FillRowHeaderColumns(dgv);
            FillMainRows(dgv);
            FillRowSumColumn(dgv);
            FillFooterRow(dgv);
        }

        abstract protected string FormatMatrixElement(int iValue);
        abstract protected void FillRowHeaderColumns(DataGridView dgv);
        abstract protected void FillHeaderRows(DataGridView dgv);
    }
}
