using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Globals
    {
        private static DateTimeFormatInfo P_dfi;
        private static Calendar P_cal;

        public static DatabaseWrapper DatabaseWrapper;

        public async static Task Initialize(IUnityContainer Container, Action<String> tick)
        {
            P_dfi = DateTimeFormatInfo.CurrentInfo;
            P_cal = P_dfi.Calendar;
            DatabaseWrapper = new DatabaseWrapper();
            await DatabaseWrapper.ReadTables(Container, tick);
        }

        public static string Format(DateTime date)
            => $"{ date:dd.MM.yyyy}";

        public static int GetMonth(DateTime date)
            => P_cal.GetMonth( date );

        public static string GetMonthName(int iMonth)
            => P_dfi.GetMonthName(iMonth).ToString();

        public static int GetWeekOfYear(DateTime date)
            => P_cal.GetWeekOfYear(date, P_dfi.CalendarWeekRule, P_dfi.FirstDayOfWeek);

        public static int GetWeekOfYear(DateTime date, int iNrOfWeeks)
            => GetWeekOfYear(date.Add(new TimeSpan(TimeSpan.TicksPerDay * iNrOfWeeks * 7)));

        public static TimeRange GetMonthRange(DateTime dat, int idMonth)
        {
            int iNrOfMonths = dat.Month + idMonth - 1;
            int iYear       = dat.Year + iNrOfMonths / 12;
            int iMonth      = iNrOfMonths % 12 + 1;
            DateTime datStart = new DateTime(iYear, iMonth, 1);
            DateTime datEnd   = datStart.AddDays(DateTime.DaysInMonth(iYear, iMonth) - 1);
            return new TimeRange(datStart, datEnd);
        }

        public static string GetTimeRangeDescription(TimeRange timeRange)
            => Format(timeRange.P_datStart) + " - " + Format(timeRange.P_datEnd);

        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static string GetVersionString(System.Reflection.Assembly a)
        {
            string versionString;
            var version = a.GetName().Version;
            var buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
                TimeSpan.TicksPerSecond * 2 * version.Revision)); // seconds since midnight, (multiply by 2 to get original)
                                                                  // a valid date-string can now be constructed like this
            versionString = String.Format("Version: {0}.{1}.{2}.{3} ", version.Major, version.Minor, version.Build, version.Revision).PadRight(30);
            versionString += String.Format("Build time: {0:G}", buildDateTime);
            return versionString;
        }

        public static string GetFullVersionInfo()
        {
            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            string strResult = string.Empty;
            foreach (var a in assemblies)
            {
                string name = a.GetName().Name;
                if (name.Substring(0, 4) == "AVF.")
                    strResult += name.PadRight(50) + Globals.GetVersionString(a) + "\n";
            }
            return strResult;
        }
    }
}
