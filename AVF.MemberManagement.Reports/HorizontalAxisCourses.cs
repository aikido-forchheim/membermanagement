using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisCourses : HorizontalAxis
    {
        public HorizontalAxisCourses()
            : base(additionalElements: 1)     // 1 additional column for trainings without course
        {
            P_ActiveElementsOnly = false;
            P_AxisType = new AxisTypeCourse();
        }

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => ModelRange();  

        private int GetModelIndexFromDbIndex(int iDbIndex)
            => iDbIndex + AdditionalAxisElements;

        private int GetDbIndexFromModelIndex(int iModelIndex)
        {
            Debug.Assert(iModelIndex > 0);
            return iModelIndex - AdditionalAxisElements;
        }

        protected override int GetModelIndexFromId(int idKurs)
            => (idKurs == 0)  // special case: training without course
               ? 0
               : GetModelIndexFromDbIndex(Globals.DatabaseWrapper.m_kurs.FindIndex(kurs => idKurs == kurs.Id));

        private int GetCourseIdFromModelIndex(int iModelCol)
            => (iModelCol == 0)  // special case: training without course
               ? 0
               : Globals.DatabaseWrapper.m_kurs[GetDbIndexFromModelIndex(iModelCol)].Id;

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int idKurs = GetCourseIdFromModelIndex(iModelCol);

            m_DbIds[iDgvCol - P_startIndex] = idKurs;

            dgv.Columns[iDgvCol].HeaderText = Globals.GetCourseDescription(idKurs);
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
