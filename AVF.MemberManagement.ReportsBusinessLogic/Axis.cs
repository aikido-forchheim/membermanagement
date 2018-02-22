using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class Axis
    {
        public virtual int MaxDatabaseId { get; } = 0;

        public virtual int MinDatabaseId { get; } = 0;

        private int DatabaseIdRange()
            => MaxDatabaseId - MinDatabaseId + 1;

        protected virtual int AdditionalAxisElements { get; set; } = 0;

        public int ModelRange()
            => DatabaseIdRange() + AdditionalAxisElements;  // 1 additional column for trainings without course

        public bool ActiveElementsOnly { get; protected set; } = false;

        public abstract int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn);

        protected virtual int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => 0;
    }
}
