using System.Windows.Forms;
using System.Collections.Generic;

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

        public override void FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);

            int id = GetDbIdFromDgvIndex(iDgvRow);

            dgv[0, iDgvRow].Value = id;  // column 0 is always reserved for key value

            List<string> list = axisType.GetDescription(id);
            for ( int iCol = 1; iCol <= axisType.HeaderStrings.Count; iCol++)
                dgv[iCol, iDgvRow].Value = list[iCol-1];
        }
    }
}
