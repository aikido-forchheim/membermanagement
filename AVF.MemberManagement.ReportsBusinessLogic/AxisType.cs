using System;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class AxisType
    {
        public abstract string MouseCellEvent(DateTime datStart, DateTime datEnd, int id, bool action);

        protected int P_MaxDbId { get; set; }

        protected int P_MinDbId { get; set; }

        public bool P_ActiveElementsOnly { get; protected set; } = false;

        public abstract int GetModelIndexFromId(int id);

        public abstract int GetIdFromModelIndex(int id);

        public abstract int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn);

        public abstract string GetDescription(int id, char separator);

        public int DatabaseIdRange()
            => P_MaxDbId - P_MinDbId + 1;

        public int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetModelIndexFromId(GetIdFromTrainingsParticipation(tn));
    }
}
