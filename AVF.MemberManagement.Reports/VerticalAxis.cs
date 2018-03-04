using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
         public abstract void FillKeyHeaderCells(DataGridView dgv);

         public void Initialize(int axisLength) { }

         public int GetDbIdFromDgvIndex(DataGridView dgv, int iDgvRow)
             => (int)dgv.Rows[iDgvRow - P_startIndex].Cells[0].Value;

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
            => (int)(dgv[0, iDgvRow].Value = axisType.GetIdFromModelIndex(iModelRow));
    }
}
