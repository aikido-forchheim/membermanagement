using System;
using System.Linq;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportBusinessLogic
{
    public abstract class ReportBase
    {
        protected DatabaseWrapper m_db;

        protected DateTime m_datStart;
        protected DateTime m_datEnd;

        protected int m_iNrOfCols;  // nr of cols of core matrix, without key/sum columns
        protected int m_iNrOfRows;  // nr of rows of core matrix, without header/footer rows

        protected class Row : IComparable<Row>
        {
            public int idRow;
            public int iRowSum;
            public int[] aiValues;

            public int CompareTo(Row other) => other.iRowSum - iRowSum;
        };

        protected Row[] m_Rows;
        protected int[] m_iColSum;
        protected int   m_iSumSum;

        protected int m_iNrOfHeaderRows;
        protected int m_iNrOfColsOnLeftSide;
        protected int m_iNrOfColsOnRightSide;

        private string[,] m_stringMatrix;

        private DataGridView m_dataGridView = new DataGridView();

        protected string[,] StringMatrix { get => m_stringMatrix; set => m_stringMatrix = value; }
        protected DataGridView ReportDataGridView { get => m_dataGridView; set => m_dataGridView = value; }

        public ReportBase(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
        {
            m_db = db;
            m_datStart = datStart;
            m_datEnd = datEnd;
        }

        protected void Initialize( int iNrOfRows, int iNrOfCols )
        {
            m_iNrOfRows = iNrOfRows;
            m_iNrOfCols = iNrOfCols;

            m_Rows = new Row[m_iNrOfRows];

            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
            {
                m_Rows[iRow] = new Row
                {
                    aiValues = new int[m_iNrOfCols],
                    idRow = iRow + 1,   // id 0 not used in database
                    iRowSum = 0
                };
            }

            m_iColSum = new int[m_iNrOfCols];
            m_iSumSum = 0;
        }

        protected void CollectData()
        {
            foreach (var trainingsTeilnahme in m_db.TrainingsTeilnahme(m_datStart, m_datEnd))
            {
                if (IsRelevant(trainingsTeilnahme))
                {
                    int iRow = RowIndexFromTrainingParticipation(trainingsTeilnahme);
                    int iCol = ColIndexFromTrainingParticipation(trainingsTeilnahme);
                    ++m_Rows[iRow].aiValues[iCol];
                    ++m_Rows[iRow].iRowSum;
                    ++m_iColSum[iCol];
                    ++m_iSumSum;
                }
            }

            int iNrOfStringRows = m_Rows   .Count(r => r.iRowSum > 0) + m_iNrOfHeaderRows + 1;  // one footer row
            int iNrOfStringCols = m_iColSum.Count(c =>         c > 0) + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;

            StringMatrix = new string[iNrOfStringRows, iNrOfStringCols];
            ReportDataGridView.ColumnCount = iNrOfStringCols;
            ReportDataGridView.RowCount = iNrOfStringRows;
        }

        protected void FillCourseHeaderRows()
        {
            int iStringCol = m_iNrOfColsOnLeftSide;
            for (int iCol = 0; iCol < m_iNrOfCols - 1; ++iCol)
            {
                if (m_iColSum[iCol] > 0)
                {
                    Kurs kurs = m_db.KursFromId(iCol + 1);
                    StringMatrix[0, iStringCol] = $"    { m_db.WeekDay(kurs.WochentagID).Substring(0, 2) } ";
                    StringMatrix[1, iStringCol] = $" {kurs.Zeit:hh}:{kurs.Zeit:mm} ";
                    ReportDataGridView.Rows[0].Cells[iStringCol].Value = $"    { m_db.WeekDay(kurs.WochentagID).Substring(0, 2) } ";
                    ReportDataGridView.Rows[1].Cells[iStringCol].Value = $" {kurs.Zeit:hh}:{kurs.Zeit:mm} ";
                    ++iStringCol;
                }
            }
            StringMatrix[0, iStringCol] = " Lehrgänge";
            StringMatrix[1, iStringCol] = " Sondertr.";
            ReportDataGridView.Rows[0].Cells[iStringCol].Value = " Lehrgänge";
            ReportDataGridView.Rows[1].Cells[iStringCol].Value = " Sondertr.";
            ++iStringCol;

            StringMatrix[0, iStringCol] = "";
            StringMatrix[1, iStringCol] = " Summe";
            ReportDataGridView.Rows[0].Cells[iStringCol].Value = "";
            ReportDataGridView.Rows[1].Cells[iStringCol].Value = " Summe";
        }

        protected void FillMainRows()
        {
            int iStringRow = m_iNrOfHeaderRows;

            for (int iRow = 0; iRow < m_Rows.Length; ++iRow)
            {
                if (m_Rows[iRow].iRowSum > 0)
                {
                    int iStringCol = 1;
                    StringMatrix[iStringRow, 0] = FormatFirstColElement(iRow);
                    ReportDataGridView.Rows[iStringRow].Cells[0].Value = FormatFirstColElement(iRow);
                    for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                    {
                        if (m_iColSum[iCol] > 0)
                        {
                            StringMatrix[iStringRow, iStringCol] = FormatMatrixElement(m_Rows[iRow].aiValues[iCol]);
                            ReportDataGridView.Rows[iStringRow].Cells[iStringCol].Value = FormatMatrixElement(m_Rows[iRow].aiValues[iCol]);
                            ++iStringCol;
                        }
                    }
                    StringMatrix[iStringRow, iStringCol] = "  " + Utilities.FormatNumber(m_Rows[iRow].iRowSum);
                    ReportDataGridView.Rows[iStringRow].Cells[iStringCol].Value = "  " + Utilities.FormatNumber(m_Rows[iRow].iRowSum);
                    ++iStringRow;
                }
            }
        }

        protected void FillFooterRow( string description )
        {
            int iStringRow = StringMatrix.GetLength(0) - 1;  // one footer row
            int iStringCol = m_iNrOfColsOnLeftSide;

            StringMatrix[iStringRow, 0] = description;
            ReportDataGridView.Rows[iStringRow].Cells[0].Value = description;
            for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
            {
                if (m_iColSum[iCol] > 0)
                {
                    StringMatrix[iStringRow, iStringCol] = FormatColSumElement(m_iColSum[iCol]);
                    ReportDataGridView.Rows[iStringRow].Cells[iStringCol].Value = FormatColSumElement(m_iColSum[iCol]);
                    ++iStringCol;
                }
            }
            StringMatrix[iStringRow, iStringCol] = "  " + Utilities.FormatNumber(m_iSumSum);
            ReportDataGridView.Rows[iStringRow].Cells[iStringCol].Value = "  " + Utilities.FormatNumber(m_iSumSum);
        }

        abstract protected bool IsRelevant(TrainingsTeilnahme tn);
        abstract protected int  RowIndexFromTrainingParticipation(TrainingsTeilnahme tn);
        abstract protected int  ColIndexFromTrainingParticipation(TrainingsTeilnahme tn);
        abstract protected string FormatFirstColElement(int iRow);
        abstract protected string FormatMatrixElement(int iValue);
        abstract protected string FormatColSumElement(int iValue);
        abstract protected void FillHeaderRows();
//        abstract protected int GetFirstColWidth();
    }
}
