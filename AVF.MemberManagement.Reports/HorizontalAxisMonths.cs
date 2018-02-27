using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisMonths : HorizontalAxis
    {
        public HorizontalAxisMonths()
            => P_AxisType = new AxisTypeMonth();

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => DatabaseIdRange();

        protected override int GetModelIndexFromId(int idMonth)
            => idMonth - P_AxisType.P_MinDbId;

        protected override int GetIdFromModelIndex(int iModelCol)
           => iModelCol + P_AxisType.P_MinDbId;

        public override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int idMonth = GetIdFromModelIndex(iModelCol);
            m_DbIds[iDgvCol - P_startIndex] = iModelCol + P_AxisType.P_MinDbId;

            dgv.Columns[iDgvCol].HeaderText = Globals.GetMonthName(idMonth);
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
