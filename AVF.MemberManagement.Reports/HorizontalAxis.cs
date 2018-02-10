using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        protected HorizontalAxis()
        {
            NrOfLeadingElements  = 0; // header not counted, is part of DataGridView
            NrOfTrailingElements = 1; // 1 row for column sum
        }

        public int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => ActiveElementsOnly ? tpModel.GetNrOfActiveCols() : NrOfSrcElements;

        protected abstract void FillHeaderCell(DataGridView dgv, int iDgvCol, int iCol);

        public abstract void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iStartIndex);

        public void FillMainHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iStartIndex)
        {
            int iDgvCol = iStartIndex;
            tpModel.ForAllCols
                (
                    action: iCol =>
                    {
                        dgv.Columns[iDgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        if (iCol > 0)
                        {
                            FillHeaderCell(dgv, iDgvCol++, iCol);
                        }
                    },
                    activeColsOnly: ActiveElementsOnly
                );
        }

        public void FillSumCells(DataGridView dgv, TrainingParticipationModel tpModel, int iNrOfColsOnLeftSide)
        {
            int iDgvRow = dgv.RowCount - 1;  // one footer row

            dgv[iNrOfColsOnLeftSide - 1, iDgvRow].Value = "Summe";

            int iDgvCol = iNrOfColsOnLeftSide;
            tpModel.ForAllCols
            (
                action: iCol => dgv[iDgvCol++, iDgvRow].Value = tpModel.GetColSum(iCol),
                activeColsOnly: ActiveElementsOnly
            );

            dgv[dgv.ColumnCount - 1, iDgvRow].Value = tpModel.GetSumSum();
        }
    }
}
