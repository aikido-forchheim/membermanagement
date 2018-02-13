using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisCourses : HorizontalAxis
    {
        public HorizontalAxisCourses()
            => ActiveElementsOnly = false;

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => DatabaseIdRange() + 1;  // 1 additional column for trainings without course

        public override int MaxDatabaseId
            => Globals.DatabaseWrapper.MaxKursNr();

        public override int MinDatabaseId
            => Globals.DatabaseWrapper.MinKursNr();

        public override int ModelRange()
            => DatabaseIdRange() + 1;  // 1 additional column for trainings without course

        protected override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);

        private int GetModelIndexFromDbIndex( int iDbIndex )
            => iDbIndex + 1;  // + 1 for special case: training without course

        private int GetDbIndexFromModelIndex( int iModelIndex )
        {
            Debug.Assert(iModelIndex > 0);
            return iModelIndex - 1;  // - 1 for special case: training without course
        }

        protected override int GetModelIndexFromId( int idKurs )
            => (idKurs == 0)  // special case: training without course
               ? 0
               : GetModelIndexFromDbIndex( Globals.DatabaseWrapper.m_kurs.FindIndex(kurs => idKurs == kurs.Id) );

        private int GetCourseIdFromModelIndex( int iModelCol )
            => (iModelCol == 0)  // special case: training without course
               ? 0
               : Globals.DatabaseWrapper.m_kurs[GetDbIndexFromModelIndex(iModelCol)].Id;

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iDgvStartIndex)
        {
            dgv.Columns[0].HeaderText = "Kalender\nwoche";

            FillMainHeaderCells(dgv, tpModel, iDgvStartIndex);

            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
        }

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int    idKurs = GetCourseIdFromModelIndex(iModelCol);
            string text;

            if (idKurs == 0)
            {
                text = "Lehrg.\netc.";
            }
            else
            {
                Kurs   kurs   = Globals.DatabaseWrapper.KursFromId(idKurs);
                string day    = Globals.DatabaseWrapper.WeekDay(kurs.WochentagID).Substring(0, 2);
                text = $"{ day }\n{kurs.Zeit:hh}:{kurs.Zeit:mm}";
            }
            dgv.Columns[iDgvCol].HeaderText = text;
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
