using System.Windows.Forms;
using System.Collections.Generic;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class VerticalAxis : Axis
    {
        public virtual void FillKeyHeaderCells(DataGridView dgv, AxisType axisType)
        {
            dgv.Columns[0].HeaderText = "Nr";

            for (int iCol = 1; iCol <= axisType.HeaderStrings.Count; iCol++)
                dgv.Columns[iCol].HeaderText = axisType.HeaderStrings[iCol - 1];

            for (int iCol = 0; iCol <= axisType.HeaderStrings.Count; iCol++)
                dgv.Columns[iCol].HeaderCell.ToolTipText = $"Klicken um nach {dgv.Columns[iCol].HeaderText} zu sortieren";
        }

        public override void FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);

            int id = GetDbIdFromDgvIndex(iDgvRow);

            dgv[0, iDgvRow].Value = id;  // column 0 is always reserved for key value

            List<string> list = axisType.GetDescription(id, Globals.TEXT_ORIENTATION.SPECIAL);
            for (int iCol = 1; iCol <= axisType.HeaderStrings.Count; iCol++)
            {
                DataGridViewCell cell = dgv[iCol, iDgvRow];
                cell.Value = list[iCol - 1];
                cell.ToolTipText = axisType.MouseAxisEvent(id, false);
            }
        }
    }
}
