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

        protected int m_iNrOfHeaderRows;
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

        protected int GetNrOfRows()
            => m_coreReport.GetNrOfActiveRows() + m_iNrOfHeaderRows + 1;  // one footer row

        protected int GetNrOfCols()
            => m_coreReport.GetNrOfActiveCols() + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;

        protected void FillCourseHeaderRows(DataGridView dgv)
        {
            int iStringCol = m_iNrOfColsOnLeftSide;

            dgv[iStringCol, 0].Value = " Lehrgänge";
            dgv[iStringCol, 1].Value = " Sondertr.";
            ++iStringCol;

            for (int iCol = 0; iCol < m_coreReport.m_iNrOfCols - 1; ++iCol)
            {
                if (m_coreReport.GetColSum(iCol) > 0)
                {
                    Kurs kurs = m_db.KursFromId(iCol + 1);
                    dgv[iStringCol, 0].Value = $"    { m_db.WeekDay(kurs.WochentagID).Substring(0, 2) } ";
                    dgv[iStringCol, 1].Value = $" {kurs.Zeit:hh}:{kurs.Zeit:mm} ";
                    ++iStringCol;
                }
            }
            dgv[iStringCol, 0].Value = "";
            dgv[iStringCol, 1].Value = " Summe";
        }

        protected void FillMainRows(DataGridView dgv)
        {
            int iStringRow = m_iNrOfHeaderRows;

            for (int iRow = 0; iRow < m_coreReport.m_iNrOfRows; ++iRow)
            {
                if (m_coreReport.GetRowSum(iRow) > 0)
                {
                    int iStringCol = 1;
                    dgv[0,iStringRow].Value = FormatFirstColElement(iRow);
                    for (int iCol = 0; iCol < m_coreReport.m_iNrOfCols; ++iCol)
                    {
                        if (m_coreReport.GetColSum(iCol) > 0)
                        {
                            dgv[iStringCol,iStringRow].Value = FormatMatrixElement(m_coreReport.GetCell( iRow, iCol ));
                            ++iStringCol;
                        }
                    }
                    dgv[iStringCol, iStringRow].Value = "  " + Utilities.FormatNumber(m_coreReport.GetRowSum(iRow));
                    ++iStringRow;
                }
            }
        }

        protected void FillFooterRow(DataGridView dgv,string description )
        {
            int iStringRow = dgv.RowCount - 1;  // one footer row
            int iStringCol = m_iNrOfColsOnLeftSide;

            dgv[0,iStringRow].Value = description;
            for (int iCol = 0; iCol < m_coreReport.m_iNrOfCols; ++iCol)
            {
                if (m_coreReport.GetColSum(iCol) > 0)
                {
                    dgv[iStringCol, iStringRow].Value = FormatColSumElement(m_coreReport.GetColSum(iCol));
                    ++iStringCol;
                }
            }
            dgv[iStringCol, iStringRow].Value = "  " + Utilities.FormatNumber(m_coreReport.GetSumSum());
        }

        abstract protected string FormatFirstColElement(int iRow);
        abstract protected string FormatMatrixElement(int iValue);
        abstract protected string FormatColSumElement(int iValue);
        abstract protected void   FillHeaderRows();
    }
}
