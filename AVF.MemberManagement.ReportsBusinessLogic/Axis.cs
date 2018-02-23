using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class Axis
    {
        protected Axis(int additionalElements = 0)
            => AdditionalAxisElements = additionalElements;

        public AxisType P_AxisType { get; set; }

        public int P_MaxDatabaseId()
            => P_AxisType.P_MaxDbId;

        public int P_MinDatabaseId()
            => P_AxisType.P_MinDbId;

        private int DatabaseIdRange()
            => P_MaxDatabaseId() - P_MinDatabaseId() + 1;

        protected virtual int AdditionalAxisElements { get; private set; } = 0;

        public int ModelRange()
            => DatabaseIdRange() + AdditionalAxisElements; 

        public bool P_ActiveElementsOnly { get; protected set; } = false;

        public abstract int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn);
    }
}
