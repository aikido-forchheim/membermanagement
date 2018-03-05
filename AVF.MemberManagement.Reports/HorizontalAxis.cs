using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public class HorizontalAxis : Axis
    {
        protected int[] m_DbIds;

        public void Initialize(int axisLength)
            => m_DbIds = new int[axisLength];

        public int GetDbIdFromDgvIndex(int iDgvCol)
            => m_DbIds[iDgvCol - P_startIndex];

        public override void FillMainKeyCell(DataGridView dgv, int iDgvCol, int iModelCol, AxisType axisType)
        {
            int id = axisType.GetIdFromModelIndex(iModelCol);

            dgv.Columns[iDgvCol].HeaderText = axisType.GetDescription(id, 1);
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;

            m_DbIds[iDgvCol - P_startIndex] = id;  // remember id 
        }
    }
}
