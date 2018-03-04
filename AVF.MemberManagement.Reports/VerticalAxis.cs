using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
         public virtual void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "Nr";

        public void Initialize(int axisLength) { }

         public int GetDbIdFromDgvIndex(DataGridView dgv, int iDgvRow)
             => (int)dgv.Rows[iDgvRow - P_startIndex].Cells[0].Value;

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
            => (int)(dgv[0, iDgvRow].Value = axisType.GetIdFromModelIndex(iModelRow));
    }
}
