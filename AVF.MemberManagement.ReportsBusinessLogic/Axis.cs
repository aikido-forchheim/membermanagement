using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class Axis
    {
        public AxisType P_AxisType { get; set; }

        public int DatabaseIdRange()
            => P_AxisType.P_MaxDbId - P_AxisType.P_MinDbId + 1;

        public abstract int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn);
    }
}
