﻿using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeek
    {
        public static Panel GetReport(int year, int week)
        {
            DateTime datStart = Globals.FirstDateOfWeekISO8601(year, week);
            DateTime datEnd   = datStart.AddDays(6);
            return new ReportMemberVsCourses(datStart, datEnd);
        }
    }
}
