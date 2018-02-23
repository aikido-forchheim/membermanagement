using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisMonths : HorizontalAxis
    {
        public HorizontalAxisMonths()
        {
            P_ActiveElementsOnly = false;
            P_AxisType = new AxisTypeMonth();
        }

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => ModelRange();

        protected override int GetModelIndexFromId(int idTraining)
        {
            Debug.Assert(idTraining > 0);
            Training training  = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            int      iMonth    = Globals.GetMonth(training.Termin);
            int      iModelCol = iMonth - P_MinDatabaseId();
            return iModelCol;
        }

        private int GetMonthIdFromModelIndex(int iModelCol)
           => iModelCol + P_MinDatabaseId();

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int idMonth = GetMonthIdFromModelIndex(iModelCol);
            m_DbIds[iDgvCol - P_startIndex] = iModelCol + P_MinDatabaseId();

            dgv.Columns[iDgvCol].HeaderText = Globals.GetMonthName(idMonth);
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
