using System;
using System.Linq;
using System.Collections.Generic;
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

        private List<TrainingsTeilnahme> m_listRelevantTrainingParticipations;

        public TrainingParticipationReport(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
        {
            m_db = db;
            m_listRelevantTrainingParticipations = m_db.TrainingsTeilnahme(datStart, datEnd);
        }

        public void Initialize
        (
            Axis xAxis,
            Axis yAxis,
            Func<TrainingsTeilnahme, bool> isRelevant
        )
        {
            m_iNrOfRows = yAxis.GetNrOfSrcElements();
            m_iNrOfCols = xAxis.GetNrOfSrcElements();

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

            foreach (var trainingsTeilnahme in m_listRelevantTrainingParticipations)
            {
                if (isRelevant(trainingsTeilnahme))
                {
                    int iRow = yAxis.GetIndexFromTrainingsParticipation(trainingsTeilnahme);
                    int iCol = xAxis.GetIndexFromTrainingsParticipation(trainingsTeilnahme);
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

        public void ForAllColumns(Action<int> action, bool activeColumnsOnly  )
        {
            for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                if ( !activeColumnsOnly || ( GetColSum(iCol) > 0 ) )
                    action(iCol);
        }

        public void ForAllRows(Action<int> action, bool activeRowsOnly)
        {
            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
                if (!activeRowsOnly || (GetRowSum(iRow) > 0))
                    action(iRow);
        }

        public void ForAllActiveCells(Action<int, int> action)
        {
            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
            {
                if (GetRowSum(iRow) > 0)
                {
                    for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                    {
                        if (GetCell(iRow, iCol) != 0)
                            action(iRow, iCol);
                    }
                }
            }
        }
        public void ForAllCells(Action<int, int> action)
        {
            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
                for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                    action(iRow, iCol);
        }
    }
}
