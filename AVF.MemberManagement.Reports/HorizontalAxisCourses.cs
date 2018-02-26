using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisCourses : HorizontalAxis
    {
        public HorizontalAxisCourses()
        {
            P_ActiveElementsOnly = false;
            P_AxisType = new AxisTypeCourse();
        }

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => DatabaseIdRange();  

        protected override int GetModelIndexFromId(int idKurs)
            => Globals.DatabaseWrapper.m_kurs.FindIndex(kurs => idKurs == kurs.Id);

        private int GetCourseIdFromModelIndex(int iModelCol)
            => Globals.DatabaseWrapper.m_kurs[iModelCol].Id;

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int idKurs = GetCourseIdFromModelIndex(iModelCol);

            m_DbIds[iDgvCol - P_startIndex] = idKurs;

            dgv.Columns[iDgvCol].HeaderText = Globals.GetCourseDescription(idKurs);
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
