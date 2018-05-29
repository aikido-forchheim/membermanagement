using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class HorizontalAxis : Axis
    {
        public override void FillMainKeyCell(DataGridView dgv, int iDgvCol, int iModelCol, AxisType axisType)
        {
            base.FillMainKeyCell(dgv, iDgvCol, iModelCol, axisType);

            int id = GetDbIdFromDgvIndex(iDgvCol);

            DataGridViewColumn col = dgv.Columns[iDgvCol];
            col.HeaderText = axisType.GetFullDesc(id, Globals.TEXT_ORIENTATION.VERTICAL);
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            col.HeaderCell.ToolTipText = axisType.MouseAxisEvent(id, false);
        }
    }
}
