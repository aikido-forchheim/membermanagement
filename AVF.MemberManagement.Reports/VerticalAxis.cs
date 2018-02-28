using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class VerticalAxis : Axis
    {
        public int GetRowKey(int iDgvRow)
            => m_DbIds[iDgvRow - P_startIndex];

        public abstract void FillKeyHeaderCells(DataGridView dgv);
    }
}
