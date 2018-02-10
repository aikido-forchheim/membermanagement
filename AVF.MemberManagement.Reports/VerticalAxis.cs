using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        protected VerticalAxis() 
            => NrOfTrailingElements = 1;  // 1 column for row sum

        protected abstract void FillHeaderCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iRow);

        public int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => ActiveElementsOnly ? tpModel.GetNrOfActiveRows() : NrOfSrcElements;

        public abstract void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iStartIndex);

        public void FillMainHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iStartIndex)
        {
            int iDgvRow = iStartIndex;
            tpModel.ForAllRows
            (
                action: iRow => FillHeaderCell(tpModel, dgv, iDgvRow++, iRow),
                activeRowsOnly: ActiveElementsOnly
            );
        }

        public void FillSumCells(DataGridView dgv, TrainingParticipationModel tpModel, int iStartIndex)
        {
            int iDgvRow = iStartIndex;
            tpModel.ForAllRows
            (
                iRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = tpModel.GetRowSum(iRow),
                ActiveElementsOnly
            );
        }
    }
}
