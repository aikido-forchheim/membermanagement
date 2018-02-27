using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        public int P_NrOfKeyColumns { get; protected set; }

        public int P_KeyColumn { get; protected set; }

        public abstract void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow);

        public abstract void FillKeyHeaderCells(DataGridView dgv);
    }
}
