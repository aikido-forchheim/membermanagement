using System;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class AxisType
    {
        public abstract string MouseCellEvent(DateTime datStart, DateTime datEnd, int id, bool action);

        public abstract int P_MaxDbId { get; }

        public abstract int P_MinDbId { get; }

        public bool P_ActiveElementsOnly { get; protected set; } = false;

        public abstract int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn);

        public abstract string GetDescription(int id);
    }
}
