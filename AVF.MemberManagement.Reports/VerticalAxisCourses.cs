using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisCourses : VerticalAxis
    {
        public VerticalAxisCourses() 
        {
            P_NrOfKeyColumns = 2;
            P_KeyColumn = 0;
            P_AxisType = new AxisTypeCourse();
        }

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => P_AxisType.GetIdFromTrainingsParticipation(tn) - P_AxisType.P_MinDbId;

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Kurs";
            dgv.Columns[1].HeaderText = "Termin";
        }

        public override void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow)
        {
            dgv[0, iDgvRow].Value = iModelRow;
            dgv[1, iDgvRow].Value = P_AxisType.GetDescription(iModelRow, ' ');
        }
    }
}
