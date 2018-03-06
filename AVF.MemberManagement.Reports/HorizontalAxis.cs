using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public class HorizontalAxis : Axis
    {
        protected int[] P_DbIds { get; set; }

        public void Initialize(int axisLength)
            => P_DbIds = new int[axisLength];

        public int GetDbIdFromDgvIndex(int iDgvCol)
            => P_DbIds[iDgvCol - P_startIndex];

        public override void FillMainKeyCell(DataGridView dgv, int iDgvCol, int iModelCol, AxisType axisType)
        {
            int id = axisType.GetIdFromModelIndex(iModelCol);

            dgv.Columns[iDgvCol].HeaderText = axisType.GetFullDesc(id, '\n');
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;

            P_DbIds[iDgvCol - P_startIndex] = id;  // remember id 
        }
    }
}
