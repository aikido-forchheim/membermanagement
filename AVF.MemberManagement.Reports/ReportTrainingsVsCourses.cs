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
                new AxisTypeCourse(),
                new AxisTypeTraining(),
                filter: tn => true
            );

            ReportFormPopulate();
        }
    }
}
