using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        public static int NrOfLeadingElements { get; protected set; }
        public static int NrOfTrailingElements { get; protected set; }

        public int GetNrOfAdditionalElements()
            => NrOfLeadingElements + NrOfTrailingElements;

        protected VerticalAxis() 
            => NrOfTrailingElements = 1;  // 1 column for row sum

        protected abstract void FillHeaderCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow);

        public int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => ActiveElementsOnly ? tpModel.GetNrOfActiveRows() : DatabaseIdRange();

        public abstract void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel);

        public void FillMainHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvRow = StartIndex;
            tpModel.ForAllRows
            (
                action: iModelRow => FillHeaderCell(tpModel, dgv, iDgvRow++, iModelRow),
                activeRowsOnly: ActiveElementsOnly
            );
        }

        public void FillSumCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvRow = StartIndex;
            tpModel.ForAllRows
            (
                iModelRow => dgv[dgv.ColumnCount - 1, iDgvRow++].Value = tpModel.GetRowSum(iModelRow),
                ActiveElementsOnly
            );
        }
    }
}
