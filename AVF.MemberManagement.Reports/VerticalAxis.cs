using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        public int NrOfKeyColumns { get; protected set; }

        public int KeyColumn { get; protected set; }

        protected abstract void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow);

        public abstract int GetNrOfDgvRows(TrainingParticipationModel tpModel);

        public abstract void FillKeyHeaderCells(DataGridView dgv);

        public abstract string MouseKeyCellEvent(DateTime datStart, DateTime datEnd, int id, bool action);

        public void FillMainKeyCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvRow = 0;
            tpModel.ForAllRows
            (
                action: iModelRow => FillMainKeyCell(tpModel, dgv, iDgvRow++, iModelRow),
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
