using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class AxisTypeTime : AxisType
    {
        public enum Period { TRAINING, WEEK, MONTH, YEAR };

        protected Period m_period;

        public AxisTypeTime(ReportDescriptor desc)
             : base(desc)
        {
            P_ActiveElementsOnly = false;
            P_MinDbId = 0;
        }

        public override int GetModelIndexFromId(int id)
            => id;

        public override int GetIdFromModelIndex(int iModelIndex)
            => iModelIndex;
    }
}
