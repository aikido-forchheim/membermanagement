using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisTrainings : HorizontalAxis
    {
        public HorizontalAxisTrainings()
        { 
            P_ActiveElementsOnly = true;
            P_AxisType = new AxisTypeTraining();
        }

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => tpModel.GetNrOfActiveCols();

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

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol)
        {
            int  idTraining = GetIdFromModelIndex(iModelCol);

            m_DbIds[iDgvCol - P_startIndex] = idTraining;

            dgv.Columns[iDgvCol].HeaderText = Globals.GetTrainingDescription(idTraining);
            dgv.Columns[iDgvCol].MinimumWidth = 10;
            dgv.Columns[iDgvCol].DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
