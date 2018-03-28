using System;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class TimeRange
    {
        public TimeRange()
        {
            P_datStart = new DateTime(0);
            P_datEnd = new DateTime(0);
        }

        public TimeRange(DateTime dat)
        {
            P_datStart = dat;
            P_datEnd = dat;
        }

        public TimeRange(DateTime datStart, DateTime datEnd)
        {
            P_datStart = datStart;
            P_datEnd = datEnd;
        }

        public DateTime P_datStart { get; set; }
        public DateTime P_datEnd   { get; set; }
    }
}
