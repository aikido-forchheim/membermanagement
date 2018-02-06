using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class Axis
    {
        protected DatabaseWrapper             m_db;
        protected TrainingParticipationReport m_coreReport;

        protected Axis(DatabaseWrapper db, TrainingParticipationReport coreReport)
        {
            m_db = db;
            m_coreReport = coreReport;
        }
    }
}
