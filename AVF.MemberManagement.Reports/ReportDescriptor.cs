using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportDescriptor
    {
        public ReportDescriptor(TimeRange timeRange, int idMember, int idCourse, int idTraining)
        {
            P_timeRange = timeRange;
            P_idMember = idMember;
            P_idCourse = idCourse;
            P_idTraining = idTraining;
        }

        public TimeRange P_timeRange { get; private set; }
        public int P_idMember { get; private set; }
        public int P_idCourse { get; private set; }
        public int P_idTraining { get; private set; }
    }
}
