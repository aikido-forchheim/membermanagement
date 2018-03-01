using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeEmpty : AxisType
    {
        public AxisTypeEmpty()
        {
            P_ActiveElementsOnly = false;
            P_MaxDbId = 0;
            P_MinDbId = 0;
        }

        public override VerticalAxis GetVerticalAxis()
            => null;

        public override int GetModelIndexFromId(int id) 
            => 0;

        public override int GetIdFromModelIndex(int iModelCol) 
            => 0;

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idMonth, bool action)
            => String.Empty;

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => 0;

        public override string GetDescription(int idWeek, char separator)
            => String.Empty;
    }
}
