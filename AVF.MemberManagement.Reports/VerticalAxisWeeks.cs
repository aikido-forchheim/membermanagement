using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisWeeks : VerticalAxis
    {
        public VerticalAxisWeeks(DateTime datStart, DateTime datEnd)
        {
            P_NrOfKeyColumns = 1;
            P_KeyColumn = 0;
            P_AxisType = new AxisTypeWeek(datStart, datEnd);
        }

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => P_AxisType.GetIdFromTrainingsParticipation(tn) - P_AxisType.P_MinDbId;

        public override void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "KW";

        public override void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow) 
            => dgv[0, iDgvRow].Value = iModelRow + P_AxisType.P_MinDbId;
    }
}
