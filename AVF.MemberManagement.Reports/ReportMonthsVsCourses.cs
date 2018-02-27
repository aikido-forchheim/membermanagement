using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMonthsVsCourses : ReportTrainingsParticipation
    {
        public ReportMonthsVsCourses(DateTime datStart, DateTime datEnd, int idMember)
        {
            CreateModel
            (
                datStart, datEnd,
                new HorizontalAxisCourses(),
                new VerticalAxisMonths(datStart, datEnd),
                filter: tn => idMember == tn.MitgliedID
            );

            m_label1.Text = "Trainingsteilnahme " + (new AxisTypeMember()).GetDescription(idMember, ' ');
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }
    }
}
