using System;
using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisTrainings : HorizontalAxis
    {
        public HorizontalAxisTrainings( ) 
            => ActiveElementsOnly = true;

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

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel m_tpModel)
        {
            dgv.Columns[0].HeaderText = "Monat\nTag";
            FillMainHeaderCells(dgv, m_tpModel);
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int  idTraining = GetIdFromModelIndex(iModelCol);

            m_DbIds[iDgvCol - StartIndex] = idTraining;

            dgv.Columns[iDgvCol].HeaderText = Globals.GetTrainingDescription(idTraining);
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgv.Columns[iDgvCol].MinimumWidth = 10;
            dgv.Columns[iDgvCol].DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
    }
}
