using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeekVsCourses : ReportTrainingsParticipation
    {
        public ReportWeekVsCourses(DateTime datStart, DateTime datEnd, int idMember)
        {
            CreateModel
            (
                datStart, datEnd,
                new HorizontalAxisCourses(),
                new VerticalAxisWeeks(),
                filter: tn => idMember == tn.MitgliedID
            );

            m_label1.Text = "Trainingsteilnahme " + (new AxisTypeMember()).GetDescription(idMember);
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }
    }
}
