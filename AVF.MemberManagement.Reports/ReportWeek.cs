using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeek
    {
        public static ReportBase GetReport(int year, int week, int idMember = Globals.ALL_MEMBERS)
        {
            DateTime  datStart  = Globals.FirstDateOfWeekISO8601(year, week);
            DateTime  datEnd    = datStart.AddDays(6);
            TimeRange timeRange = new TimeRange(datStart, datEnd);
            return new ReportTrainingsParticipation(typeof(AxisTypeCourse), typeof(AxisTypeMember), timeRange, idMember);
        }
    }
}
