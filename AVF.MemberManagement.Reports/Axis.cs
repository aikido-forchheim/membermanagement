using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class Axis
    {
        protected int[] m_DbIds;

        public AxisType P_AxisType { get; set; }

        public int P_NrOfKeyColumns { get; protected set; }

        public int P_startIndex { get; set; } // axis starts at this column

        public void Initialize( int axisLength )
            => m_DbIds = new int[axisLength];

        public int GetDbIdFromDgvIndex(int iDgvIndex)
            => m_DbIds[iDgvIndex - P_startIndex];

        public virtual int FillMainKeyCell(DataGridView dgv, int iDgvIndex, int iModelIndex)
            => m_DbIds[iDgvIndex - P_startIndex] = P_AxisType.GetIdFromModelIndex(iModelIndex);
    }
}
