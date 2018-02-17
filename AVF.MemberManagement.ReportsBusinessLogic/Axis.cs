using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class Axis
    {
        public int StartIndex { get; set; } // Axis starts at this position

        public abstract int MaxDatabaseId { get; }

        public abstract int MinDatabaseId { get; }

        public int DatabaseIdRange()
            => MaxDatabaseId - MinDatabaseId + 1;

        public abstract int ModelRange();

        public bool ActiveElementsOnly { get; protected set; }

        public abstract int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn);
    }
}
