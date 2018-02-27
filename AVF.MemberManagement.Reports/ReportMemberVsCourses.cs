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
                datStart, datEnd,
                new HorizontalAxisCourses(), 
                new VerticalAxisMembers(),
                filter: tn => true
            );

            m_label1.Text = "Überblick Trainingsteilnahme";
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }

        protected override string MouseMainDataAreaCellEvent(DateTime datStart, DateTime datEnd, int idMember, int idKurs, bool action )
        {
            return action
                ? Show( new ReportMemberVsTrainings(m_datStart, m_datEnd, idKurs) )
                : $"Klicken für Details zur Teilnahme von\n" 
                     + m_yAxis.P_AxisType.GetDescription( idMember, ' ') 
                     + $" am Kurs\n" 
                     + m_xAxis.P_AxisType.GetDescription( idKurs, ' ');
        }
    }
}
