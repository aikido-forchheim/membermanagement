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

        public const TimeRange ALL_TIMERANGE = null;
        public const int ALL_MEMBERS   = -1;  // legal member ids start with 1, -1 means "all members"
        public const int ALL_COURSES   = -1;  // legal course ids start with 0, -1 means "all courses"
        public const int ALL_TRAININGS = -1;  // legal training ids start with 0, -1 means "all trainings"
        public const int ALL_MONTHS = -1;
        public const int ALL_YEARS = -1;

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

        public static string GetTimeRangeDescription(TimeRange timeRange)
        {
            String str = Format(timeRange.P_datStart);
            if (!timeRange.IsOneDay())
                str += " - " + Format(timeRange.P_datEnd);
            return str;
        }

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

        public static TimeRange GetYearRange(ReportDescriptor desc, int idYear)
        {
            DateTime dat = desc.P_timeRange.P_datStart;
            DateTime datStart = new DateTime(desc.P_idYear, 1, 1);
            DateTime datEnd = datStart.AddYears(idYear - 1);
            return new TimeRange(datStart, datEnd);
        }

        public static TimeRange GetMonthRange(ReportDescriptor desc, int idMonth)
        {
            DateTime dat = desc.P_timeRange.P_datStart;
            int iNrOfMonths = dat.Month + idMonth - 1;
            int iYear = dat.Year + iNrOfMonths / 12;
            int iMonth = iNrOfMonths % 12 + 1;
            DateTime datStart = new DateTime(iYear, iMonth, 1);
            DateTime datEnd = datStart.AddDays(DateTime.DaysInMonth(iYear, iMonth) - 1);
            return new TimeRange(datStart, datEnd);
        }

        public static TimeRange GetWeekRange(ReportDescriptor desc, int idWeek)
        {
            DateTime dat  = desc.P_timeRange.P_datStart;
            int      year = dat.Year;
            int      week = Globals.GetWeekOfYear(dat, idWeek);
            DateTime datStart = Globals.FirstDateOfWeekISO8601(year, week);
            DateTime datEnd = datStart.AddDays(6);
            return new TimeRange(datStart, datEnd);
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
