using System;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeEmpty : AxisType
    {
        public AxisTypeEmpty(DateTime datStart, DateTime datEnd)
             : base(datStart, datEnd)
        {
            P_ActiveElementsOnly = false;
            P_MaxDbId = 0;
            P_MinDbId = 0;
        }

        public override int GetModelIndexFromId(int id) 
            => 0;

        public override int GetIdFromModelIndex(int iModelCol) 
            => 0;

        public override string MouseAxisEvent(int idMonth, bool action)
            => String.Empty;

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => 0;

        public override string GetDescription(int id, int iNr = 1)
            => String.Empty;
    }
}
