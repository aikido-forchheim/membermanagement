using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportDescriptor
    {
        public ReportDescriptor
        (
            TimeRange timeRange = Globals.ALL_YEARS,
            int idMember = Globals.ALL_MEMBERS,
            int idCourse = Globals.ALL_COURSES,
            int idTraining = Globals.ALL_TRAININGS,
            int idMonth = Globals.ALL_MONTHS
        )
        {
            P_timeRange = timeRange;
            P_idMember = idMember;
            P_idCourse = idCourse;
            P_idTraining = idTraining;
            P_idMonth = idMonth;
        }

        public TimeRange P_timeRange { get; private set; }
        public int P_idMember { get; private set; }
        public int P_idCourse { get; private set; }
        public int P_idTraining { get; private set; }
        public int P_idMonth { get; private set; }
    }
}
