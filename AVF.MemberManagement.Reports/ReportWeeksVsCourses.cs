using System;

namespace AVF.MemberManagement.Reports
{
    class ReportWeeksVsCourses : ReportTrainingsParticipation
    {
        public ReportWeeksVsCourses(DateTime datStart, DateTime datEnd)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeCourse(P_datStart, P_datEnd),
                new AxisTypeWeek(P_datStart, P_datEnd),
                filter: tn => true
            );

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(-1);   // -1: all members
            P_labelMonat.Text  = new AxisTypeMonth(datStart, datEnd).GetDescription(0);
            ReportFormPopulate();
        }
    }
}
