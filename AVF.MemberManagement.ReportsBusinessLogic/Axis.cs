using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class Axis
    {
        public int NrOfLeadingElements  { get; protected set; }
        public int NrOfTrailingElements { get; protected set; }

        public int GetMrOfAdditionalElements( )
            => NrOfLeadingElements + NrOfTrailingElements;

        public bool ActiveElementsOnly { get; protected set; }

        public abstract int NrOfSrcElements { get; }

        public abstract int GetIndexFromTrainingsParticipation(TrainingsTeilnahme tn);
    }
}
