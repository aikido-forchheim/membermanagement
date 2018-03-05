using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class Axis
    {
        public int P_startIndex { get; set; } // axis starts at this column

        public abstract void FillMainKeyCell(DataGridView dgv, int iDgvIndex, int iModelIndex, AxisType axisType);
    }
}
