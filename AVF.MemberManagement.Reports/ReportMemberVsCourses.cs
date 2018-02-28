using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourses : ReportTrainingsParticipation
    {
        public ReportMemberVsCourses( DateTime datStart, DateTime datEnd )
        {
            CreateModel
            (
                bHide: false,
                datStart, datEnd,
                new AxisTypeCourse(),
                new AxisTypeMember(),
                new VerticalAxisMembers(),
                filter: tn => true
            );

            m_labelReportName.Text = "Überblick Trainingsteilnahme";
        }

        protected override string MouseMainDataAreaCellEvent(DateTime datStart, DateTime datEnd, int idMember, int idKurs, bool action )
        {
            return action
                ? Show( new ReportMemberVsTrainings(m_datStart, m_datEnd, idKurs) )
                : $"Klicken für Details zur Teilnahme von\n" 
                     + AxisTypeMember.GetDesc( idMember, ' ') 
                     + $" am Kurs\n" 
                     + AxisTypeCourse.GetDesc( idKurs, ' ');
        }
    }
}
