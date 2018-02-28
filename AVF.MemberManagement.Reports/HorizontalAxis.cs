using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public HorizontalAxis()
            => P_NrOfKeyColumns = 0;

        public bool P_Hide { get; protected set; } = false;

        public override int FillMainKeyCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int id = base.FillMainKeyCell(dgv, iDgvCol, iModelCol);
            dgv.Columns[iDgvCol].HeaderText = P_AxisType.GetDescription(id, '\n');
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            return id;
        }
    }
}
