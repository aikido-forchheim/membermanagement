using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportDescriptor
    {
        public ReportDescriptor(TimeRange timeRange, int idMember, int idCourse)
        {
            P_timeRange = timeRange;
            P_idMember = idMember;
            P_idCourse = idCourse;
        }

        public TimeRange P_timeRange { get; private set; }
        public int P_idMember { get; private set; }
        public int P_idCourse { get; private set; }
    }
}
