using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public abstract class Axis
    {
        protected DatabaseWrapper             m_db;
        protected TrainingParticipationMatrix m_tpMatrix;

        protected Axis(DatabaseWrapper db, TrainingParticipationMatrix tpMatrix)
        {
            m_db = db;
            m_tpMatrix = tpMatrix;
        }

        public abstract int GetNrOfSrcElements();
        public abstract int GetIndexFromTrainingsParticipation(TrainingsTeilnahme tn);
    }
}
