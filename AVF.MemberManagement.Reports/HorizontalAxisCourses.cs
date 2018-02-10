using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisCourses : HorizontalAxis
    {
        public HorizontalAxisCourses() 
            => ActiveElementsOnly = false;

        public override int NrOfSrcElements => Globals.DatabaseWrapper.MaxKursNr() + 1;

        public override int GetIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iStartIndex)
        {
            dgv.Columns[0].HeaderText = "Kalender\nwoche";

            dgv.Columns[iStartIndex].HeaderText = "Lehrg.\netc.";
            dgv.Columns[iStartIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            FillMainHeaderCells(dgv, tpModel, iStartIndex + 1);
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
            dgv.Columns[dgv.ColumnCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iCol)
        {
            Kurs kurs = Globals.DatabaseWrapper.KursFromId(iCol);
            dgv.Columns[iDgvCol].HeaderText = $"{ Globals.DatabaseWrapper.WeekDay(kurs.WochentagID).Substring(0, 2) }\n{kurs.Zeit:hh}:{kurs.Zeit:mm}";
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
