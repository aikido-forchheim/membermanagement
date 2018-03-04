using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class Axis
    {
        public int P_NrOfKeyColumns { get; protected set; }

        public int P_startIndex { get; set; } // axis starts at this column

        public abstract int FillMainKeyCell(DataGridView dgv, int iDgvIndex, int iModelIndex, AxisType axisType);
    }
}
