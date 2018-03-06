using System;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourses : ReportTrainingsParticipation
    {
        public ReportMemberVsCourses( DateTime datStart, DateTime datEnd )
            : base( datStart, datEnd )
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeCourse(P_datStart, P_datEnd),
                new AxisTypeMember(P_datStart, P_datEnd),
                filter: tn => true
            );

            P_Info0.Text = P_axisTypeMember.GetFullDesc(-1);   // -1: all members
            ReportFormPopulate();
        }

        protected override string MouseMainDataAreaCellEvent(DateTime datStart, DateTime datEnd, int idMember, int idKurs, bool action )
            => action
                ? ReportMain.SwitchToPanel( new ReportMemberVsTrainings(P_datStart, P_datEnd, idKurs) )
                : $"Klicken für Details zur Teilnahme von\n"
                     + P_axisTypeMember.GetFullDesc(idMember)
                     + $" am Kurs\n" 
                     + P_xAxisType.GetDescription(idKurs);
    }
}
