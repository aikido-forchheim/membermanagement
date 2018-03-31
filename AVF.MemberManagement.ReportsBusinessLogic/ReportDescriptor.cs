using System;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class ReportDescriptor
    {
        public ReportDescriptor
        (
            ReportDescriptor desc = null,
            Type xAxisType = null,
            Type yAxisType = null,
            TimeRange timeRange = Globals.ALL_TIMERANGE,
            int idMember = Globals.ALL_MEMBERS,
            int idCourse = Globals.ALL_COURSES,
            int idTraining = Globals.ALL_TRAININGS,
            int idWeek = Globals.ALL_WEEKS,
            int idMonth = Globals.ALL_MONTHS,
            int idYear = Globals.ALL_YEARS
        )
        {
            if (desc == null)
            {
                P_xAxisType = xAxisType;
                P_yAxisType = yAxisType;
                P_timeRange = timeRange;
                P_idMember = idMember;
                P_idCourse = idCourse;
                P_idTraining = idTraining;
                P_idWeek = idWeek;
                P_idMonth = idMonth;
                P_idYear = idYear;
            }
            else
            {
                P_xAxisType  = (xAxisType  == null)                  ? desc.P_xAxisType : xAxisType;
                P_yAxisType  = (yAxisType  == null)                  ? desc.P_yAxisType : yAxisType;
                P_timeRange  = (timeRange  == Globals.ALL_TIMERANGE) ? desc.P_timeRange : timeRange;
                P_idMember   = (idMember   == Globals.ALL_MEMBERS)   ? desc.P_idMember : idMember;
                P_idCourse   = (idCourse   == Globals.ALL_COURSES)   ? desc.P_idCourse : idCourse;
                P_idTraining = (idTraining == Globals.ALL_TRAININGS) ? desc.P_idTraining : idTraining;
                P_idWeek     = (idWeek     == Globals.ALL_WEEKS)     ? desc.P_idWeek : idWeek;
                P_idMonth    = (idMonth    == Globals.ALL_MONTHS)    ? desc.P_idMonth : idMonth;
                P_idYear     = (idYear     == Globals.ALL_YEARS)     ? desc.P_idYear : idYear;

                if (idYear != Globals.ALL_YEARS)
                    P_timeRange = Globals.GetYearRange(this, idYear);
                else if (idMonth != Globals.ALL_MONTHS)
                    P_timeRange = Globals.GetYearRange(this, idMonth);
                else if (idWeek != Globals.ALL_WEEKS)
                    P_timeRange = Globals.GetYearRange(this, idWeek);
            }
        }

        public ReportDescriptor ShallowCopy()
            => (ReportDescriptor)MemberwiseClone();

        public Type      P_xAxisType  { get; private set; } = null;
        public Type      P_yAxisType  { get; private set; } = null;
        public TimeRange P_timeRange  { get; private set; } = Globals.ALL_TIMERANGE;
        public int       P_idMember   { get; private set; } = Globals.ALL_MEMBERS;
        public int       P_idCourse   { get; private set; } = Globals.ALL_COURSES;
        public int       P_idTraining { get; private set; } = Globals.ALL_TRAININGS;
        public int       P_idWeek     { get; private set; } = Globals.ALL_WEEKS;
        public int       P_idMonth    { get; private set; } = Globals.ALL_MONTHS;
        public int       P_idYear     { get; private set; } = Globals.ALL_YEARS;
    }
}
