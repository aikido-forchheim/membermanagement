using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class Axis : IAxis
    {
        protected int[] m_DbIds;

        public AxisType P_AxisType { get; set; }

        public int P_NrOfKeyColumns { get; protected set; }

        public int P_startIndex { get; set; } // axis starts at this column

        public void Initialize()
            => m_DbIds = new int[DatabaseIdRange()];

        public int DatabaseIdRange()
            => P_AxisType.P_MaxDbId - P_AxisType.P_MinDbId + 1;

        public abstract int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn);

        public virtual int FillMainKeyCell(DataGridView dgv, int iDgvIndex, int iModelIndex)
            => m_DbIds[iDgvIndex - P_startIndex] = P_AxisType.GetIdFromModelIndex(iModelIndex);
    }
}
