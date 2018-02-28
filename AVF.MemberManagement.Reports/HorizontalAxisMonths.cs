using System;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisMonths : HorizontalAxis
    {
        public HorizontalAxisMonths(DateTime datStart, DateTime datEnd)
            => P_AxisType = new AxisTypeMonth(datStart, datEnd);
    }
}
