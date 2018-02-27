using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public int P_startIndex { get; set; } // horizontal axis starts at this column

        public bool P_Hide { get; protected set; } = false;

        protected int[] m_DbIds;

        public void Initialize( )
            => m_DbIds = new int[DatabaseIdRange()];

        public int GetColKey(int iDgvCol)
            => m_DbIds[iDgvCol - P_startIndex];

        protected abstract int GetModelIndexFromId(int id);

        protected abstract int GetIdFromModelIndex(int id);

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetModelIndexFromId(P_AxisType.GetIdFromTrainingsParticipation(tn));

        public abstract int GetNrOfDgvCols(TrainingParticipationModel tpModel);

        public void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int id = GetIdFromModelIndex(iModelCol);

            m_DbIds[iDgvCol - P_startIndex] = id;

            dgv.Columns[iDgvCol].HeaderText = P_AxisType.GetDescription(id, '\n');
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
