using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisMonths : HorizontalAxis
    {
        public HorizontalAxisMonths(DateTime datStart, DateTime datEnd)
            => P_AxisType = new AxisTypeMonth(datStart, datEnd);

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => DatabaseIdRange();

        protected override int GetModelIndexFromId(int idMonth)
            => idMonth - P_AxisType.P_MinDbId;

        protected override int GetIdFromModelIndex(int iModelCol)
           => iModelCol + P_AxisType.P_MinDbId;
    }
}
