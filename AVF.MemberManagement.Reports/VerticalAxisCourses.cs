using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisCourses : VerticalAxis
    {
        public VerticalAxisCourses() 
        {
            P_NrOfKeyColumns = 1;
            P_AxisType = new AxisTypeCourse();
        }

        public override void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "Termin";

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow);
            dgv[0, iDgvRow].Value = AxisTypeCourse.GetDesc(id, ' ');
            return id;
        }
    }
}
