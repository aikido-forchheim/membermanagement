using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public class VerticalAxis : Axis
    {
        public virtual void FillKeyHeaderCells(DataGridView dgv, AxisType axisType)
        {
            dgv.Columns[0].HeaderText = "Nr";
            for (int iCol = 1; iCol <= axisType.HeaderStrings.Count; iCol++)
                dgv.Columns[iCol].HeaderText = axisType.HeaderStrings[iCol - 1];
        }

        public void Initialize(int axisLength) { }

        public int GetDbIdFromDgvIndex(DataGridView dgv, int iDgvRow)
             => (int)dgv.Rows[iDgvRow - P_startIndex].Cells[0].Value;

        public override void FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            int id = axisType.GetIdFromModelIndex(iModelRow);

            dgv[0, iDgvRow].Value = id;  // column 0 is always reserved for key value

            for ( int iCol = 1; iCol <= axisType.HeaderStrings.Count; iCol++)
                dgv[iCol, iDgvRow].Value = axisType.GetDescription(id, iCol);
        }
    }
}
