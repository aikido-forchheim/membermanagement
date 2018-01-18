using System;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.BusinessLogic
{
    public abstract class MatrixReport
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
        protected int m_iNrOfFooterRows;
        protected int m_iNrOfColsOnLeftSide;
        protected int m_iNrOfColsOnRightSide;

        protected string[,] m_stringMatrix;

        public MatrixReport(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
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

        protected void PrepareData()
        {
            foreach (var trainingsTeilnahme in m_db.TrainingsTeilnahme(m_datStart, m_datEnd))
            {
                if (IsRelevant(trainingsTeilnahme))
                {
                    int iRow = RowIndexFromTrainingParticipationTrainingParticipation(trainingsTeilnahme);
                    int iCol = ColIndexFromTrainingParticipation(trainingsTeilnahme);
                    ++m_Rows[iRow].aiValues[iCol];
                    ++m_Rows[iRow].iRowSum;
                    ++m_iColSum[iCol];
                    ++m_iSumSum;
                }
            }

            Array.Sort(m_Rows);

            int iNrOfStringRows = m_Rows.Count(r => r.iRowSum > 0) + m_iNrOfHeaderRows + m_iNrOfFooterRows;
            int iNrOfStringCols = m_iColSum.Count(c => c > 0) + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;

            m_stringMatrix = new string[iNrOfStringRows, iNrOfStringCols];
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

        protected void FillFooterRows()
        {
            int iStringRow = m_stringMatrix.GetLength(0) - m_iNrOfFooterRows;
            int iStringCol = m_iNrOfColsOnLeftSide;

            m_stringMatrix[iStringRow, 0] = "                     Insgesamt  ";
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
        abstract protected int  RowIndexFromTrainingParticipationTrainingParticipation(TrainingsTeilnahme tn);
        abstract protected int  ColIndexFromTrainingParticipation(TrainingsTeilnahme tn);
        abstract protected string FormatFirstColElement(int i);
        abstract protected string FormatMatrixElement(int i);
        abstract protected string FormatColSumElement(int i);
        abstract protected void FillHeaderRows();
    }
}
