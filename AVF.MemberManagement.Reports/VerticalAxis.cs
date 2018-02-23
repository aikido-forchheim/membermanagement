using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        public int P_NrOfKeyColumns { get; protected set; }

        public int P_KeyColumn { get; protected set; }

        protected VerticalAxis(int additionalElements = 0) : base(additionalElements) { }

        protected abstract void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow);

        public abstract int GetNrOfDgvRows(TrainingParticipationModel tpModel);

        public abstract void FillKeyHeaderCells(DataGridView dgv);

        public void FillMainKeyCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvRow = 0;
            tpModel.ForAllRows
            (
                action: iModelRow => FillMainKeyCell(tpModel, dgv, iDgvRow++, iModelRow),
                activeRowsOnly: P_ActiveElementsOnly
            );
        }

        public void FillSumCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvRow = 0;
            tpModel.ForAllRows
            (
                iModelRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = tpModel.GetRowSum(iModelRow),
                P_ActiveElementsOnly
            );
        }
    }
}
