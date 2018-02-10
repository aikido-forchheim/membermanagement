using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeekVsCourse : ReportForm
    {
        public ReportWeekVsCourse(DateTime datStart, DateTime datEnd, int idMember)
            : base(datStart, datEnd)
        {
            m_xAxis = new HorizontalAxisCourses();
            m_yAxis = new VerticalAxisWeeks();

            m_tpModel = new TrainingParticipationModel
            (
                datStart,
                datEnd,
                m_xAxis,
                m_yAxis,
                (tn => idMember == tn.MitgliedID)
            );
        }
    }
}
