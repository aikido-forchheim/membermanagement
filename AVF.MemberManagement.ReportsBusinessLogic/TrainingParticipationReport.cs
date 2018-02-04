using System;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class TrainingParticipationReport
    {
        private DatabaseWrapper m_db;

        private int m_iNrOfCols;
        private int m_iNrOfRows;

        private class Row : IComparable<Row>
        {
            public int idRow;
            public int iRowSum;
            public int[] aiValues;

            public int CompareTo(Row other) => other.iRowSum - iRowSum;
        };

        private Row[] m_Rows;
        private int[] m_iColSum;
        private int   m_iSumSum;

        public TrainingParticipationReport(DatabaseWrapper db)
        {
            m_db = db;
        }

        public void Initialize
        (
            DateTime datStart, 
            DateTime datEnd,
            int iNrOfRows, 
            int iNrOfCols,
            Func<TrainingsTeilnahme, bool> isRelevant,
            Func<TrainingsTeilnahme, int> rowIndex,
            Func<TrainingsTeilnahme, int> colIndex
        )
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

            foreach (var trainingsTeilnahme in m_db.TrainingsTeilnahme(datStart, datEnd))
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

        public void SortRows()
            => Array.Sort(m_Rows);

        public int GetNrOfActiveRows()
            => m_Rows.Count(r => r.iRowSum > 0);

        public int GetNrOfActiveCols()
            => m_iColSum.Count(c => c > 0);

        public int GetCell(int iRow, int iCol)
            => m_Rows[iRow].aiValues[iCol];

        public int GetRowSum(int iRow)
            => m_Rows[iRow].iRowSum;

        public int GetColSum(int iCol)
            => m_iColSum[iCol];

        public int GetSumSum()
            => m_iSumSum;

        public int GetRowId(int iRow)
            => m_Rows[iRow].idRow;

        public void ForAllActiveColumns(Action<int> action)
        {
            for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                if (GetColSum(iCol) > 0)
                    action(iCol);
        }

        public void ForAllActiveRows(Action<int> action)
        {
            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
                if (GetRowSum(iRow) > 0)
                    action(iRow);
        }

        public void ForAllActiveCells(Action<int,int> action)
        {
            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
            {
                if (GetRowSum(iRow) > 0)
                {
                    for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                    {
                        int iCellValue = GetCell(iRow, iCol);
                        if (iCellValue != 0)
                            action(iRow, iCol);
                    }
                }
            }
        }
    }
}
