using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class Axis
    {
        public AxisType P_AxisType { get; set; }

        public int P_MaxDatabaseId()
            => P_AxisType.P_MaxDbId;

        public int P_MinDatabaseId()
            => P_AxisType.P_MinDbId;

        public int DatabaseIdRange()
            => P_MaxDatabaseId() - P_MinDatabaseId() + 1;

        public bool P_ActiveElementsOnly { get; protected set; } = false;

        public abstract int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn);
    }
}
