using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public class HorizontalAxis : Axis
    {
        public override void FillMainKeyCell(DataGridView dgv, int iDgvCol, int iModelCol, AxisType axisType)
        {
            base.FillMainKeyCell(dgv, iDgvCol, iModelCol, axisType);

            int id = GetDbIdFromDgvIndex(iDgvCol);

            dgv.Columns[iDgvCol].HeaderText = axisType.GetFullDesc(id, "\n");
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
