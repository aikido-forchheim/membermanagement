using System;
using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisCourses : VerticalAxis
    {
        public VerticalAxisCourses()
        {
            NrOfKeyColumns = 2;
            KeyColumn = 0;
            ActiveElementsOnly = false;
            AdditionalAxisElements = 1;  // 1 additional column for trainings without course
        }

        public override int MaxDatabaseId
            => Globals.DatabaseWrapper.MaxKursNr();

        public override int MinDatabaseId
            => Globals.DatabaseWrapper.MinKursNr();

        public override int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => ModelRange(); 

        protected override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
        {
            int idKurs = Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);
            return (idKurs == 0) ? 0 : (idKurs - MinDatabaseId + 1);
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

        public override string MouseKeyCellEvent(DateTime datStart, DateTime datEnd, int idKurs, bool action)
        {
            return action
                ? ReportTrainingsParticipation.Show(new ReportMemberVsTrainings(datStart, datEnd, idKurs))
                : $"Klicken für Details zum Kurs\n" + Globals.GetCourseDescription(idKurs);
        }
    }
}
