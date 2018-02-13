using System;
using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisTrainings : HorizontalAxis
    {
        private DateTime m_datStart;
        private DateTime m_datEnd;
        private int      m_idKurs;

        public HorizontalAxisTrainings
        (
            DateTime datStart, 
            DateTime datEnd, 
            int idKurs
        )
        {
            m_datStart = datStart;
            m_datEnd   = datEnd;
            m_idKurs   = idKurs;
            ActiveElementsOnly = true;
        }

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => tpModel.GetNrOfActiveCols();

        public override int MaxDatabaseId
            => Globals.DatabaseWrapper.MaxTrainingNr();

        public override int MinDatabaseId
            => Globals.DatabaseWrapper.MinTrainingNr();

        public override int ModelRange()
            => DatabaseIdRange();

        protected override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;

        private int GetModelIndexFromDbIndex(int iDbIndex)
            => iDbIndex;

        private int GetDbIndexFromModelIndex(int iModelIndex)
            => iModelIndex;

        protected override int GetModelIndexFromId(int idTraining)
        {
            int dbIndex = Globals.DatabaseWrapper.m_trainings.FindIndex(training => idTraining == training.Id);
            return GetModelIndexFromDbIndex(dbIndex);
        }

        private int GetIdFromModelIndex(int iModelCol)
        {
            int dbIndex = GetDbIndexFromModelIndex(iModelCol);
            return Globals.DatabaseWrapper.m_trainings[dbIndex].Id;
        }

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel m_tpModel, int iDgvStartIndex)
        {
            dgv.Columns[0].HeaderText = "Monat\nTag";
            FillMainHeaderCells(dgv, m_tpModel, iDgvStartIndex);
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
        }

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int dbIndex = GetDbIndexFromModelIndex(iModelCol);
            Training training = Globals.DatabaseWrapper.m_trainings[dbIndex];  
            dgv.Columns[iDgvCol].HeaderText = $"{training.Termin:dd}.\n{training.Termin:MM}.";
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
