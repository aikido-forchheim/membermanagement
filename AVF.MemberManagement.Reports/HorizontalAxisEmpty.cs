using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    // HorizontalAxisEmpty is used for one dimensional forms, which have no horizontal axis

    class HorizontalAxisEmpty : HorizontalAxis
    {
        public HorizontalAxisEmpty()
        {
            P_AxisType = new AxisTypeEmpty();
            P_Hide = true;
        }

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel) => 1;

        protected override int GetModelIndexFromId(int id) => 0;

        protected override int GetIdFromModelIndex(int iModelCol) => 0;
    }
}
