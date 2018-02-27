using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Globals
    {
        private static DateTimeFormatInfo m_dfi;
        private static Calendar m_cal;

        public static DatabaseWrapper DatabaseWrapper;

        public async static Task Initialize(IUnityContainer Container, Action<String> tick)
        {
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
            DatabaseWrapper = new DatabaseWrapper();
            await DatabaseWrapper.ReadTables(Container, tick);
        }

        public static string Format(DateTime date)
            => $"{ date:dd.MM.yyyy}";

        public static int GetMonth(DateTime date)
            => m_cal.GetMonth( date );

        public static string GetMonthName(int iMonth)
            => m_dfi.GetMonthName(iMonth).ToString();

        public static string GetMonthNameShort(int iMonth)
            => GetMonthName(iMonth).Substring( 0, 3 );

        public static int GetWeekOfYear(DateTime date)
            => m_cal.GetWeekOfYear(date, m_dfi.CalendarWeekRule, m_dfi.FirstDayOfWeek);

        public static string GetTimeRangeDescription(DateTime datStart, DateTime datEnd)
            => Format(datStart) + ' ' + Format(datEnd);
    }
}
