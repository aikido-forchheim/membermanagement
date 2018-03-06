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

        public static DateTime FirstDayOfMonth(DateTime datStart, int idMonth)
            => new DateTime(datStart.Year, datStart.Month + idMonth, 1);

        public static DateTime LastDayOfMonth(DateTime datStart, int idMonth)
            => FirstDayOfMonth(datStart, idMonth).AddDays(DateTime.DaysInMonth(datStart.Year, datStart.Month) - 1);

        public static string GetTimeRangeDescription(DateTime datStart, DateTime datEnd)
            => Format(datStart) + ' ' + Format(datEnd);

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
    }
}
