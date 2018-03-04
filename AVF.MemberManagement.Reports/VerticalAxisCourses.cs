using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisCourses : VerticalAxis
    {
        public VerticalAxisCourses()
            => P_NrOfKeyColumns = 2;

        protected override void FillAdditionalKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[1].HeaderText = "Termin";

        protected override void FillAdditionalKeyCells(DataGridView dgv, int iDgvRow, int id)
            => dgv[1, iDgvRow].Value = AxisTypeCourse.GetDesc(id, ' ');
    }
}
