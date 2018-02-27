using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisTrainings : HorizontalAxis
    {
        public HorizontalAxisTrainings()
            => P_AxisType = new AxisTypeTraining();

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => tpModel.GetNrOfActiveCols();

        protected override int GetModelIndexFromId(int idTraining)
            => Globals.DatabaseWrapper.m_trainings.FindIndex(training => idTraining == training.Id);

        protected override int GetIdFromModelIndex(int iModelCol)
            => Globals.DatabaseWrapper.m_trainings[iModelCol].Id;
    }
}
