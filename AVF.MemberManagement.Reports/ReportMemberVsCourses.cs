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
                new AxisTypeCourse(),
                new AxisTypeMember(),
                filter: tn => true
            );

            ReportFormPopulate();
        }

        protected override string MouseMainDataAreaCellEvent(DateTime datStart, DateTime datEnd, int idMember, int idKurs, bool action )
            => action
                ? ReportMain.SwitchToPanel( new ReportMemberVsTrainings(m_datStart, m_datEnd, idKurs) )
                : $"Klicken für Details zur Teilnahme von\n" 
                     + AxisTypeMember.GetDesc( idMember, ' ') 
                     + $" am Kurs\n" 
                     + AxisTypeCourse.GetDesc( idKurs, ' ');
    }
}
