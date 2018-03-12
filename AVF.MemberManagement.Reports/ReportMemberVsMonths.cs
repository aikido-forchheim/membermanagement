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
                new AxisTypeMonth(P_datStart, P_datEnd),
                new AxisTypeMember(P_datStart, P_datEnd),
                filter: tn => true
            );

            P_Info0.Text = P_axisTypeMember.GetFullDesc(-1);   // -1: all members
            ReportFormPopulate();
        }

        protected override string MouseMainDataAreaCellEvent(DateTime datStart, DateTime datEnd, int idMember, int idMonth, bool action)
            => action
                ? ReportMain.P_formMain.SwitchToPanel(new ReportTrainingsVsCourses(P_datStart, P_datEnd, idMonth))
                : $"Klicken für Details zur Teilnahme von\n"
                     + P_axisTypeMember.GetFullDesc(idMember)
                     + $" im Monat\n"
                     + P_xAxisType.GetDescription(idMonth);
    }
}
