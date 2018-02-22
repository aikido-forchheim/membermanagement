using System;
using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisWeeks : VerticalAxis
    {
        private const int LAST_CALENDAR_WEEK = 52;
        private const int FRST_CALENDAR_WEEK =  1;

        public VerticalAxisWeeks()
        {
            NrOfKeyColumns = 1;
            KeyColumn = 0;
            ActiveElementsOnly = false;
        }

        public override int MaxDatabaseId
            => LAST_CALENDAR_WEEK;   // Not really in database, but kind of

        public override int MinDatabaseId
            => FRST_CALENDAR_WEEK;  // Not really in database, but kind of

        public override int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => ModelRange();

        protected override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
        {
            Debug.Assert(tn.TrainingID > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(tn.TrainingID);
            return Globals.GetWeekOfYear(training.Termin);
        }

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
        {
            int idWeek = GetIdFromTrainingsParticipation(tn);
            return idWeek - MinDatabaseId;
        }

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "KW";
        }

        protected override void FillMainKeyCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow) 
            => dgv[0, iDgvRow++].Value = iModelRow + MinDatabaseId;

        public override string MouseKeyCellEvent(DateTime datStart, DateTime datEnd, int id, bool action)
        {
            return action
                ? String.Empty
                : $"Klicken für Details zu Trainings in dieser Woche (Noch nicht implementiert)";
        }
    }
}
