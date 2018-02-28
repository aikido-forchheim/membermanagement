using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public interface IAxis
    {
        int DatabaseIdRange();
        int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn);
    }
}
