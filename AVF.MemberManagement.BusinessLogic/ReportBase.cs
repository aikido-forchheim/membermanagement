using System;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.BusinessLogic
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

        protected string[,] m_stringMatrix;

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

            m_stringMatrix = new string[iNrOfStringRows, iNrOfStringCols];
        }

        protected void FillCourseHeaderRows()
        {
            int iStringCol = m_iNrOfColsOnLeftSide;
            for (int iCol = 0; iCol < m_iNrOfCols - 1; ++iCol)
            {
                if (m_iColSum[iCol] > 0)
                {
                    Kurs kurs = m_db.KursFromId(iCol + 1);
                    m_stringMatrix[0, iStringCol] = $"    { m_db.WeekDay(kurs.WochentagID).Substring(0, 2) } ";
                    m_stringMatrix[1, iStringCol] = $" {kurs.Zeit:hh}:{kurs.Zeit:mm} ";
                    ++iStringCol;
                }
            }
            m_stringMatrix[0, iStringCol] = " Lehrgänge";
            m_stringMatrix[1, iStringCol] = " Sondertr.";
            ++iStringCol;

            m_stringMatrix[0, iStringCol] = "";
            m_stringMatrix[1, iStringCol] = " Summe";
        }

        protected void FillMainRows()
        {
            int iStringRow = m_iNrOfHeaderRows;

            for (int iRow = 0; iRow < m_Rows.Length; ++iRow)
            {
                if (m_Rows[iRow].iRowSum > 0)
                {
                    int iStringCol = 1;
                    m_stringMatrix[iStringRow, 0] = FormatFirstColElement(iRow);
                    for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                    {
                        if (m_iColSum[iCol] > 0)
                        {
                            m_stringMatrix[iStringRow, iStringCol] = FormatMatrixElement(m_Rows[iRow].aiValues[iCol]);
                            ++iStringCol;
                        }
                    }
                    m_stringMatrix[iStringRow, iStringCol] = "  " + Utilities.FormatNumber(m_Rows[iRow].iRowSum);
                    ++iStringRow;
                }
            }
        }

        protected void FillFooterRow( string description )
        {
            int iStringRow = m_stringMatrix.GetLength(0) - 1;  // one footer row
            int iStringCol = m_iNrOfColsOnLeftSide;

            m_stringMatrix[iStringRow, 0] = description;
            for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
            {
                if (m_iColSum[iCol] > 0)
                {
                    m_stringMatrix[iStringRow, iStringCol] = FormatColSumElement(m_iColSum[iCol]);
                    ++iStringCol;
                }
            }
            m_stringMatrix[iStringRow, iStringCol] = "  " + Utilities.FormatNumber(m_iSumSum);
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
