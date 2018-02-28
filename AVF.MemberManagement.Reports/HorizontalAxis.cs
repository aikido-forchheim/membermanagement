using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public HorizontalAxis()
            => P_NrOfKeyColumns = 0;

        public bool P_Hide { get; protected set; } = false;

        public int GetColKey(int iDgvCol)
            => m_DbIds[iDgvCol - P_startIndex];

        public int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => P_AxisType.P_ActiveElementsOnly ? tpModel.GetNrOfActiveCols() : DatabaseIdRange();

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => P_AxisType.GetModelIndexFromId(P_AxisType.GetIdFromTrainingsParticipation(tn));

        public override int FillMainKeyCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int id = base.FillMainKeyCell(dgv, iDgvCol, iModelCol);
            dgv.Columns[iDgvCol].HeaderText = P_AxisType.GetDescription(id, '\n');
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            return id;
        }
    }
}
