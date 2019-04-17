using System;
using System.Collections.Generic;
using System.Text;

namespace AVF.StandardLibrary.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsToday(this DateTime dateTime)
        {
            var today = DateTime.Now;

            return (dateTime.Day == today.Day && dateTime.Month == today.Month && dateTime.Year == today.Year);
        }
    }
}
