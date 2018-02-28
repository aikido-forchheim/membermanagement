using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisCourses : VerticalAxis
    {
        public VerticalAxisCourses() 
        {
            P_NrOfKeyColumns = 1;
            P_AxisType = new AxisTypeCourse();
        }

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => P_AxisType.GetIdFromTrainingsParticipation(tn) - P_AxisType.P_MinDbId;

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Termin";
        }

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow);
            dgv[0, iDgvRow].Value = P_AxisType.GetDescription(id, ' ');
            return id;
        }
    }
}
