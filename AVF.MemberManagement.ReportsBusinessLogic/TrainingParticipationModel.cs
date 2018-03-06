using System;
using System.Linq;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class TrainingParticipationModel
    {
        private int P_iNrOfCols;
        private int P_iNrOfRows;

        private class Row : IComparable<Row>
        {
            public int iRowSum;
            public int[] aiValues;

            public int CompareTo(Row other) => other.iRowSum - iRowSum;
        };

        private Row[] P_Rows;
        private int[] P_iColSum;

        public TrainingParticipationModel
        (
            List<TrainingsTeilnahme> tpList,
            IAxis xAxisType,
            IAxis yAxisType
        )
        {
            P_iNrOfRows = yAxisType.DatabaseIdRange();
            P_iNrOfCols = xAxisType.DatabaseIdRange();

            P_Rows = new Row[P_iNrOfRows];

            for (int iRow = 0; iRow < P_iNrOfRows; ++iRow)
            {
                P_Rows[iRow] = new Row
                {
                    aiValues = new int[P_iNrOfCols],
                    iRowSum = 0
                };
            }

            P_iColSum = new int[P_iNrOfCols];

            foreach (var trainingsParticipation in tpList)
            {
                int iRow = yAxisType.GetModelIndexFromTrainingsParticipation(trainingsParticipation);
                int iCol = xAxisType.GetModelIndexFromTrainingsParticipation(trainingsParticipation);
                ++P_Rows[iRow].aiValues[iCol];
                ++P_Rows[iRow].iRowSum;
                ++P_iColSum[iCol];
            }
        }

        public void SortRows()
            => Array.Sort(P_Rows);

        public int GetNrOfActiveRows()
            => P_Rows.Count(r => r.iRowSum > 0);

        public int GetNrOfActiveCols()
            => P_iColSum.Count(c => c > 0);

        public int GetCell(int iRow, int iCol)
            => P_Rows[iRow].aiValues[iCol];

        public int GetRowSum(int iRow)
            => P_Rows[iRow].iRowSum;

        public void ForAllCols(Action<int> action, bool activeColsOnly  )
        {
            for (int iCol = 0; iCol < P_iNrOfCols; ++iCol)
                if ( !activeColsOnly || (P_iColSum[iCol] > 0 ) )
                    action(iCol);
        }

        public void ForAllRows(Action<int> action, bool activeRowsOnly)
        {
            for (int iRow = 0; iRow < P_iNrOfRows; ++iRow)
                if (!activeRowsOnly || (GetRowSum(iRow) > 0))
                    action(iRow);
        }
    }
}
