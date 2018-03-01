using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisTrainings : VerticalAxis
    {
        public VerticalAxisTrainings()
            => P_NrOfKeyColumns = 1;

        public override void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "Training";

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);
            dgv[0, iDgvRow].Value = AxisTypeTraining.GetDesc(id, ' ');
            return id;
        }
    }
}
