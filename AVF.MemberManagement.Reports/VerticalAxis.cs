using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        public static int NrOfKeyColumns { get; protected set; }

        protected abstract void FillHeaderCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow);

        public int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => ActiveElementsOnly ? tpModel.GetNrOfActiveRows() : DatabaseIdRange();

        public abstract void FillKeyCells(DataGridView dgv, TrainingParticipationModel tpModel);

        public abstract int GetRowKey(DataGridView dgv, int row);

        public void FillMainKeyCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvRow = 0;
            tpModel.ForAllRows
            (
                action: iModelRow => FillHeaderCell(tpModel, dgv, iDgvRow++, iModelRow),
                activeRowsOnly: ActiveElementsOnly
            );
        }

        public void FillSumCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvRow = 0;
            tpModel.ForAllRows
            (
                iModelRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = tpModel.GetRowSum(iModelRow),
                ActiveElementsOnly
            );
        }
    }
}
