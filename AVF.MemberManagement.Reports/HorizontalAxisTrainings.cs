using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

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

        protected override int GetModelIndexFromId(int idTraining)
            => Globals.DatabaseWrapper.m_trainings.FindIndex(training => idTraining == training.Id);

        private int GetIdFromModelIndex(int iModelCol)
            => Globals.DatabaseWrapper.m_trainings[iModelCol].Id;

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
