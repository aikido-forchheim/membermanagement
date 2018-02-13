using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        protected VerticalAxis() 
            => NrOfTrailingElements = 1;  // 1 column for row sum

        public override int ModelRange()
            => DatabaseIdRange();

        protected abstract void FillHeaderCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow);

        public int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => ActiveElementsOnly ? tpModel.GetNrOfActiveRows() : DatabaseIdRange();

        public abstract void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iDgvStartIndex);

        public void FillMainHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iDgvStartIndex)
        {
            int iDgvRow = iDgvStartIndex;
            tpModel.ForAllRows
            (
                action: iModelRow => FillHeaderCell(tpModel, dgv, iDgvRow++, iModelRow),
                activeRowsOnly: ActiveElementsOnly
            );
        }

        public void FillSumCells(DataGridView dgv, TrainingParticipationModel tpModel, int iDgvStartIndex)
        {
            int iDgvRow = iDgvStartIndex;
            tpModel.ForAllRows
            (
                iModelRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = tpModel.GetRowSum(iModelRow),
                ActiveElementsOnly
            );
        }
    }
}
