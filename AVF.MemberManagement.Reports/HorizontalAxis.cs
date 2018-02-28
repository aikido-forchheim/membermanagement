using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class HorizontalAxis : Axis
    {
        public HorizontalAxis()
            => P_NrOfKeyColumns = 0;

        public override int FillMainKeyCell(DataGridView dgv, int iDgvCol, int iModelCol, AxisType axisType)
        {
            int id = base.FillMainKeyCell(dgv, iDgvCol, iModelCol, axisType);
            dgv.Columns[iDgvCol].HeaderText = axisType.GetDescription(id, '\n');
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            return id;
        }
    }
}
