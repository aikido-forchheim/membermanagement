using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMonths : VerticalAxis
    {
        public VerticalAxisMonths(DateTime datStart, DateTime datEnd)
        {
            P_NrOfKeyColumns = 2;
            P_KeyColumn = 0;
            P_AxisType = new AxisTypeMonth(datStart, datEnd);
        }

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => P_AxisType.GetIdFromTrainingsParticipation(tn) - P_AxisType.P_MinDbId;

        public override void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "Monat";

        public override void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow)
        {
            dgv[0, iDgvRow].Value = iModelRow;
            dgv[1, iDgvRow].Value = P_AxisType.GetDescription(iModelRow, ' ');
        }
    }
}
