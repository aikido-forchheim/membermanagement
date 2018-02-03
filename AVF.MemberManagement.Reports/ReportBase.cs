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

        protected void CollectData
        ( 
            Func<TrainingsTeilnahme, bool> isRelevant,
            Func<TrainingsTeilnahme, int>  rowIndex,
            Func<TrainingsTeilnahme, int>  colIndex
        )
        {
            foreach (var trainingsTeilnahme in m_db.TrainingsTeilnahme(m_datStart, m_datEnd))
            {
                if (isRelevant(trainingsTeilnahme))
                {
                    int iRow = rowIndex(trainingsTeilnahme);
                    int iCol = colIndex(trainingsTeilnahme);
                    ++m_Rows[iRow].aiValues[iCol];
                    ++m_Rows[iRow].iRowSum;
                    ++m_iColSum[iCol];
                    ++m_iSumSum;
                }
            }
       }

        protected int GetNrOfRows()
            => m_Rows.Count(r => r.iRowSum > 0) + m_iNrOfHeaderRows + 1;  // one footer row

        protected int GetNrOfCols()
            => m_iColSum.Count(c => c > 0) + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;

        protected void FillCourseHeaderRows(DataGridView dgv)
        {
            int iStringCol = m_iNrOfColsOnLeftSide;

            dgv[iStringCol, 0].Value = " Lehrgänge";
            dgv[iStringCol, 1].Value = " Sondertr.";
            ++iStringCol;

            for (int iCol = 0; iCol < m_iNrOfCols - 1; ++iCol)
            {
                if (m_iColSum[iCol] > 0)
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

            for (int iRow = 0; iRow < m_Rows.Length; ++iRow)
            {
                if (m_Rows[iRow].iRowSum > 0)
                {
                    int iStringCol = 1;
                    dgv[0,iStringRow].Value = FormatFirstColElement(iRow);
                    for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                    {
                        if (m_iColSum[iCol] > 0)
                        {
                            dgv[iStringCol,iStringRow].Value = FormatMatrixElement(m_Rows[iRow].aiValues[iCol]);
                            ++iStringCol;
                        }
                    }
                    dgv[iStringCol, iStringRow].Value = "  " + Utilities.FormatNumber(m_Rows[iRow].iRowSum);
                    ++iStringRow;
                }
            }
        }

        protected void FillFooterRow(DataGridView dgv,string description )
        {
            int iStringRow = dgv.RowCount - 1;  // one footer row
            int iStringCol = m_iNrOfColsOnLeftSide;

            dgv[0,iStringRow].Value = description;
            for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
            {
                if (m_iColSum[iCol] > 0)
                {
                    dgv[iStringCol, iStringRow].Value = FormatColSumElement(m_iColSum[iCol]);
                    ++iStringCol;
                }
            }
            dgv[iStringCol, iStringRow].Value = "  " + Utilities.FormatNumber(m_iSumSum);
        }

        abstract protected string FormatFirstColElement(int iRow);
        abstract protected string FormatMatrixElement(int iValue);
        abstract protected string FormatColSumElement(int iValue);
        abstract protected void FillHeaderRows();
//        abstract protected int GetFirstColWidth();
    }
}
