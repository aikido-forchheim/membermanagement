using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisWeeks : VerticalAxis
    {
        public VerticalAxisWeeks()
        {
            P_NrOfKeyColumns = 1;
            P_KeyColumn = 0;
            P_ActiveElementsOnly = false;
            P_AxisType = new AxisTypeWeek();
        }

        public override int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => DatabaseIdRange();

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => P_AxisType.GetIdFromTrainingsParticipation(tn) - P_MinDatabaseId();

        public override void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "KW";

        protected override void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow) 
            => dgv[0, iDgvRow++].Value = iModelRow + P_MinDatabaseId();
    }
}
