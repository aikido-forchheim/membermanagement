using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisTrainings : HorizontalAxis
    {
        public HorizontalAxisTrainings(DateTime datStart, DateTime datEnd)
            => P_AxisType = new AxisTypeTraining(datStart, datEnd);

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => tpModel.GetNrOfActiveCols();

        protected override int GetModelIndexFromId(int idTraining)
            => Globals.DatabaseWrapper.m_trainings.FindIndex(training => idTraining == training.Id);

        protected override int GetIdFromModelIndex(int iModelCol)
            => Globals.DatabaseWrapper.m_trainings[iModelCol].Id;
    }
}
