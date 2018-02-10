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
            public int idRow;
            public int iRowSum;
            public int[] aiValues;

            public int CompareTo(Row other) => other.iRowSum - iRowSum;
        };

        private Row[] m_Rows;
        private int[] m_iColSum;
        private int   m_iSumSum;

        public TrainingParticipationModel
        (
            DateTime datStart, 
            DateTime datEnd,
            Axis xAxis,
            Axis yAxis,
            Func<TrainingsTeilnahme, bool> filter
        )
        {
            m_iNrOfRows = yAxis.NrOfSrcElements;
            m_iNrOfCols = xAxis.NrOfSrcElements;

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

            var tnList  = Globals.DatabaseWrapper.TrainingsTeilnahme(datStart, datEnd);
            var tnList2 = Globals.DatabaseWrapper.Filter(tnList, filter);
            foreach (var trainingsTeilnahme in tnList2)
            {
                int iRow = yAxis.GetIndexFromTrainingsParticipation(trainingsTeilnahme);
                int iCol = xAxis.GetIndexFromTrainingsParticipation(trainingsTeilnahme);
                ++m_Rows[iRow].aiValues[iCol];
                ++m_Rows[iRow].iRowSum;
                ++m_iColSum[iCol];
                ++m_iSumSum;
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

        public void ForAllCols(Action<int> action, bool activeColsOnly  )
        {
            for (int iCol = 0; iCol < m_iNrOfCols; ++iCol)
                if ( !activeColsOnly || ( GetColSum(iCol) > 0 ) )
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
