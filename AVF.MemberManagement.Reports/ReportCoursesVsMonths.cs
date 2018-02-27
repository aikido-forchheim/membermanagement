using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportCoursesVsMonths : ReportTrainingsParticipation
    {
        public ReportCoursesVsMonths(DateTime datStart, DateTime datEnd, int idMember)
        {
            CreateModel
            (
                datStart, datEnd,
                new HorizontalAxisMonths(datStart, datEnd),
                new VerticalAxisCourses(),
                filter: tn => (idMember == -1) ? true : (idMember == tn.MitgliedID)
            );

            m_label1.Text = "Überblick Kurse";
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }
    }
}
