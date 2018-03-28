using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsMonths : ReportTrainingsParticipation
    {
        public ReportMemberVsMonths(DateTime datStart, DateTime datEnd, int idKurs)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                new AxisTypeMonth(P_datStart, P_datEnd),
                new AxisTypeMember(P_datStart, P_datEnd),
                filter: tn => (idKurs == -1) ? true : (idKurs == Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID))
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(-1);   // -1: all members
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
