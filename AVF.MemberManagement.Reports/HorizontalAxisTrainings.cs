using System;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisTrainings : HorizontalAxis
    {
        public HorizontalAxisTrainings(DateTime datStart, DateTime datEnd)
            => P_AxisType = new AxisTypeTraining();
    }
}
