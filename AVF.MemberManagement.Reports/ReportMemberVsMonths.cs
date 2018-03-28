using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsMonths : ReportTrainingsParticipation
    {
        public ReportMemberVsMonths(TimeRange timeRange, int idKurs)
            : base(timeRange)
        {
            CreateModel
            (
                new AxisTypeMonth(timeRange),
                new AxisTypeMember(timeRange),
                filter: tn => (idKurs == -1) ? true : (idKurs == Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID))
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(-1);   // -1: all members
            ReportFormPopulate();
        }

        protected override string MouseMainDataAreaCellEvent(TimeRange timeRange, int idMember, int idMonth, bool action)
            => action
                ? ReportMain.P_formMain.SwitchToPanel(new ReportTrainingsVsCourses(timeRange, idMonth))
                : $"Klicken für Details zur Teilnahme von\n"
                     + P_axisTypeMember.GetFullDesc(idMember)
                     + $" im Monat\n"
                     + P_xAxisType.GetDescription(idMonth);
    }
}
