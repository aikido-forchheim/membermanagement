using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        protected abstract void FillAdditionalKeyHeaderCells(DataGridView dgv);

        public virtual void FillKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Nr";
            FillAdditionalKeyHeaderCells(dgv);
        }

        public void Initialize(int axisLength) { }

         public int GetDbIdFromDgvIndex(DataGridView dgv, int iDgvRow)
             => (int)dgv.Rows[iDgvRow - P_startIndex].Cells[0].Value;

        protected abstract void FillAdditionalKeyCells(DataGridView dgv, int iDgvRow, int id);

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            int id = (int)(dgv[0, iDgvRow].Value = axisType.GetIdFromModelIndex(iModelRow));
            FillAdditionalKeyCells(dgv, iDgvRow, id);
            return id;
        }
    }
}
