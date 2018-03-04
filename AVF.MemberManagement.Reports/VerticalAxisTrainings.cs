using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisTrainings : VerticalAxis
    {
        public VerticalAxisTrainings()
            => P_NrOfKeyColumns = 2;

        protected override void FillAdditionalKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[1].HeaderText = "Training";

        protected override void FillAdditionalKeyCells(DataGridView dgv, int iDgvRow, int id)
            => dgv[1, iDgvRow].Value = AxisTypeTraining.GetDesc(id, ' ');
    }
}
