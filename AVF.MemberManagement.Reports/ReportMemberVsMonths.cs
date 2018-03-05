using System;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsMonths : ReportTrainingsParticipation
    {
        public ReportMemberVsMonths(DateTime datStart, DateTime datEnd)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeMonth(datStart, datEnd),
                new AxisTypeMember(),
                filter: tn => true
            );

            ReportFormPopulate();
        }

        protected override string MouseMainDataAreaCellEvent(DateTime datStart, DateTime datEnd, int idMember, int idMonth, bool action)
            => action
                ? ReportMain.SwitchToPanel(new ReportTrainingsVsCourses(m_datStart, m_datEnd, idMonth))
                : $"Klicken für Details zur Teilnahme von\n"
                     + m_axisTypeMember.GetFullDesc(idMember)
                     + $" im Monat\n"
                     + m_xAxisType.GetDescription(idMonth);
    }
}
