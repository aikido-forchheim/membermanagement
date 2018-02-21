using System;
using System.Linq;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class TrainingParticipationModel
    {
        private int m_iNrOfCols;
        private int m_iNrOfRows;

        private class Row : IComparable<Row>
        {
            public int iRowSum;
            public int[] aiValues;

            public int CompareTo(Row other) => other.iRowSum - iRowSum;
        };

        private Row[] m_Rows;
        private int[] m_iColSum;

        public TrainingParticipationModel
        (
            List<TrainingsTeilnahme> tpList,
            Axis xAxis,
            Axis yAxis
        )
        {
            m_iNrOfRows = yAxis.ModelRange();
            m_iNrOfCols = xAxis.ModelRange();

            m_Rows = new Row[m_iNrOfRows];

            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
            {
                m_Rows[iRow] = new Row
                {
                    aiValues = new int[m_iNrOfCols],
                    iRowSum = 0
                };
            }

            m_iColSum = new int[m_iNrOfCols];

            foreach (var trainingsParticipation in tpList)
            {
                int iRow = yAxis.GetModelIndexFromTrainingsParticipation(trainingsParticipation);
                int iCol = xAxis.GetModelIndexFromTrainingsParticipation(trainingsParticipation);
                ++m_Rows[iRow].aiValues[iCol];
                ++m_Rows[iRow].iRowSum;
                ++m_iColSum[iCol];
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

        public void ForAllCols(Action<int> action, bool activeColsOnly  )
        {
            for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                if ( !activeColsOnly || (m_iColSum[iCol] > 0 ) )
                    action(iCol);
        }

        public void ForAllRows(Action<int> action, bool activeRowsOnly)
        {
            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
                if (!activeRowsOnly || (GetRowSum(iRow) > 0))
                    action(iRow);
        }
    }
}
