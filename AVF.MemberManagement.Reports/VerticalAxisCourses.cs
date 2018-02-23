using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisCourses : VerticalAxis
    {
        public VerticalAxisCourses() 
            : base( additionalElements : 1) // 1 additional column for trainings without course
        {
            P_NrOfKeyColumns = 2;
            P_KeyColumn = 0;
            P_ActiveElementsOnly = false;
            P_AxisType = new AxisTypeCourse();
        }

        public override int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => ModelRange(); 

         public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
        {
            int idKurs = Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);
            return (idKurs == 0) ? 0 : (idKurs - P_MinDatabaseId() + 1);
        }

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Kurs";
            dgv.Columns[1].HeaderText = "Termin";
        }

        protected override void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow)
        {
            int idKurs = iModelRow;
            dgv[0, iDgvRow].Value = idKurs;
            dgv[1, iDgvRow].Value = Globals.GetCourseDescription(idKurs);
            ++iDgvRow;
        }
    }
}
