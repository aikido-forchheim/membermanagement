using System;

namespace AVF.MemberManagement.Reports
{
    class ReportMonthsVsCourses : ReportTrainingsParticipation
    {
        public ReportMonthsVsCourses(DateTime datStart, DateTime datEnd, int idMember)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                new AxisTypeCourse(P_datStart, P_datEnd),
                new AxisTypeMonth(P_datStart, P_datEnd),
                filter: tn => idMember == tn.MitgliedID
            );

            P_labelMember.Text  = P_axisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
