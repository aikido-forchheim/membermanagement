using System;

namespace AVF.MemberManagement.Reports
{
    class ReportWeeksVsCourses : ReportTrainingsParticipation
    {
        public ReportWeeksVsCourses(DateTime datStart, DateTime datEnd, int idMonth)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeCourse(),
                new AxisTypeWeek(datStart, datEnd),
                filter: tn => true
            );

            m_Info0.Text = new AxisTypeMonth(datStart, datEnd).GetDescription(0);
            ReportFormPopulate();
        }
    }
}
