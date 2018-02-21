using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisCourses : HorizontalAxis
    {
        public HorizontalAxisCourses( ) 
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

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            dgv.Columns[0].HeaderText = "Kalender\nwoche";

            FillMainHeaderCells(dgv, tpModel);

            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
        }

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int idKurs = GetCourseIdFromModelIndex(iModelCol);

            m_DbIds[iDgvCol - StartIndex] = idKurs;

            dgv.Columns[iDgvCol].HeaderText = Globals.GetCourseDescription(idKurs);
            dgv.Columns[iDgvCol].SortMode   = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
