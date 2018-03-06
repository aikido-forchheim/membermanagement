using System;

namespace AVF.MemberManagement.Reports
{
    class ReportCoursesVsMonths : ReportTrainingsParticipation
    {
        public ReportCoursesVsMonths(DateTime datStart, DateTime datEnd, int idMember)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeMonth(P_datStart, P_datEnd),
                new AxisTypeCourse(P_datStart, P_datEnd),
                filter: tn => (idMember == -1) ? true : (idMember == tn.MitgliedID)
            );

            P_Info0.Text = P_axisTypeMember.GetFullDesc(idMember);
            ReportFormPopulate();
        }
    }
}
