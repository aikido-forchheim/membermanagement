using System;
using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisMonths : HorizontalAxis
    {
        private const int LAST_MONTH = 12;
        private const int FRST_MONTH = 1;

        public HorizontalAxisMonths()
            => ActiveElementsOnly = false;

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => ModelRange();

        public override int MaxDatabaseId
            => LAST_MONTH;   // Not really in database, but kind of

        public override int MinDatabaseId
            => FRST_MONTH;  // Not really in database, but kind of

        protected override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;

        protected override int GetModelIndexFromId(int idTraining)
        {
            Debug.Assert(idTraining > 0);
            Training training  = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            int      iMonth    = Globals.GetMonth(training.Termin);
            int      iModelCol = iMonth - MinDatabaseId;
            return iModelCol;
        }

        private int GetMonthIdFromModelIndex(int iModelCol)
           => iModelCol + MinDatabaseId;

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel m_tpModel)
        {
            dgv.Columns[0].HeaderText = "Kurs";
            FillMainHeaderCells(dgv, m_tpModel);
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
        }

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int idMonth = GetMonthIdFromModelIndex(iModelCol);
            m_DbIds[iDgvCol - StartIndex] = iModelCol + MinDatabaseId;

            dgv.Columns[iDgvCol].HeaderText = Globals.GetMonthName(idMonth);
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        public override string MouseHeaderCellEvent(DateTime datStart, DateTime datEnd, int idMonth, bool action)
        {
            return action
                ? String.Empty
                : $"Klicken für Details zum Monat " + Globals.GetMonthName(idMonth) + "\n noch nicht implementiert";
        }
    }
}
