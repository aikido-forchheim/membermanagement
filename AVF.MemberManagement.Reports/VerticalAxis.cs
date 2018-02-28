using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
         public abstract void FillKeyHeaderCells(DataGridView dgv);
    }
}
