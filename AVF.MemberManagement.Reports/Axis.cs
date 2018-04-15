using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class Axis
    {
        public int P_startIndex { get; set; } // axis starts at this column

        private int[] m_DbIds;

        public void Initialize(AxisType axisType)
            => m_DbIds = new int[axisType.DatabaseIdRange()];

        public int GetDbIdFromDgvIndex(int iDgvIndex)
            => m_DbIds[iDgvIndex - P_startIndex];

        public virtual void FillMainKeyCell(DataGridView dgv, int iDgvIndex, int iModelIndex, AxisType axisType)
            => m_DbIds[iDgvIndex - P_startIndex] = axisType.GetIdFromModelIndex(iModelIndex);
    }
}
