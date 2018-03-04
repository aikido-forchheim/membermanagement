using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisTrainings : VerticalAxis
    {
        public VerticalAxisTrainings()
            => P_NrOfKeyColumns = 2;

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Nr";
            dgv.Columns[1].HeaderText = "Training";
        }

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);
            dgv[1, iDgvRow].Value = AxisTypeTraining.GetDesc(id, ' ');
            return id;
        }
    }
}
