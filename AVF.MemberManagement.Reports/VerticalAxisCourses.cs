using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisCourses : VerticalAxis
    {
        public VerticalAxisCourses()
            => P_NrOfKeyColumns = 2;

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            base.FillKeyHeaderCells(dgv);
            dgv.Columns[1].HeaderText = "Termin";
        }

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);
            dgv[1, iDgvRow].Value = AxisTypeCourse.GetDesc(id, ' ');
            return id;
        }
    }
}
