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
            TimeRange timeRange = TimeRange.UNRESTRICTED,
            int idMember = Globals.ALL_MEMBERS,
            int idCourse = Globals.ALL_COURSES,
            int idTraining = Globals.ALL_TRAININGS,
            int nrPeriod = Globals._UNDEFINED,
            Period period = Period.UNDEFINED
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
                P_nrPeriod = nrPeriod;
                P_period = period;
            }
            else
            {
                P_xAxisType  = (xAxisType  == null)                   ? desc.P_xAxisType  : xAxisType;
                P_yAxisType  = (yAxisType  == null)                   ? desc.P_yAxisType  : yAxisType;
                P_timeRange  = (timeRange  == TimeRange.UNRESTRICTED) ? desc.P_timeRange  : timeRange;
                P_idMember   = (idMember   == Globals.ALL_MEMBERS)    ? desc.P_idMember   : idMember;
                P_idCourse   = (idCourse   == Globals.ALL_COURSES)    ? desc.P_idCourse   : idCourse;
                P_idTraining = (idTraining == Globals.ALL_TRAININGS ) ? desc.P_idTraining : idTraining;
                P_nrPeriod   = (nrPeriod   == Globals._UNDEFINED)     ? desc.P_nrPeriod   : nrPeriod;
                P_period     = (period     == Period.UNDEFINED)       ? desc.P_period     : period;

                switch(P_period)
                {
                    case Period.YEAR:
                        P_timeRange = Globals.GetYearRange(this, P_nrPeriod);
                        P_nrPeriod = 0;
                        break;
                    case Period.MONTH:
                        P_timeRange = Globals.GetMonthRange(this, P_nrPeriod);
                        P_nrPeriod = 0;
                        break;
                    case Period.WEEK:
                        P_timeRange = Globals.GetWeekRange(this, P_nrPeriod);
                        P_nrPeriod = 0;
                        break;
                    case Period.ALL:
                        P_timeRange = TimeRange.UNRESTRICTED;
                        nrPeriod = Globals._UNDEFINED;
                        break;
                    case Period.UNDEFINED:
                        break;
                }

                if (P_timeRange == TimeRange.UNRESTRICTED)
                    P_timeRange = new TimeRange(Globals.DatabaseWrapper.GetStartValidData(), DateTime.Now);
            }
        }

        public ReportDescriptor ShallowCopy()
            => (ReportDescriptor)MemberwiseClone();

        public enum Period { UNDEFINED, TRAINING, WEEK, MONTH, YEAR, ALL }

        public Type      P_xAxisType  { get; private set; }
        public Type      P_yAxisType  { get; private set; }
        public TimeRange P_timeRange  { get; private set; }
        public int       P_idMember   { get; private set; }
        public int       P_idCourse   { get; private set; }
        public int       P_idTraining { get; private set; }
        public int       P_nrPeriod   { get; private set; }
        public int       P_idYear     { get; private set; }
        public Period    P_period     { get; private set; }
    }
}
