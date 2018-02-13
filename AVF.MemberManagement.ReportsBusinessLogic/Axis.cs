using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class Axis
    {
        public int NrOfLeadingElements  { get; protected set; }
        public int NrOfTrailingElements { get; protected set; }

        public int GetMrOfAdditionalElements( )
            => NrOfLeadingElements + NrOfTrailingElements;

        public abstract int MaxDatabaseId { get; }

        public abstract int MinDatabaseId { get; }

        public int DatabaseIdRange()
            => MaxDatabaseId - MinDatabaseId + 1;

        public abstract int ModelRange();

        public bool ActiveElementsOnly { get; protected set; }

        public abstract int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn);
    }
}
