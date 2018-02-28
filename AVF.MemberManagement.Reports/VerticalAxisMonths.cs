using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMonths : VerticalAxis
    {
        public VerticalAxisMonths(DateTime datStart, DateTime datEnd)
        {
            P_NrOfKeyColumns = 1;
            P_AxisType = new AxisTypeMonth(datStart, datEnd);
        }

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => P_AxisType.GetIdFromTrainingsParticipation(tn) - P_AxisType.P_MinDbId;

        public override void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "Monat";

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow);
            dgv[0, iDgvRow].Value = P_AxisType.GetDescription(id, ' ');
            return id;
        }
    }
}
