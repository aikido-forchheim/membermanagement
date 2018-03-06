using System;

namespace AVF.MemberManagement.Reports
{
    class ReportTrainingsVsCourses : ReportTrainingsParticipation
    {
        public ReportTrainingsVsCourses(DateTime datStart, DateTime datEnd, int idMonth)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeCourse(P_datStart, P_datEnd),
                new AxisTypeTraining(P_datStart, P_datEnd),
                filter: tn => true
            );

            P_Info0.Text = "Monat:";
            ReportFormPopulate();
        }
    }
}
