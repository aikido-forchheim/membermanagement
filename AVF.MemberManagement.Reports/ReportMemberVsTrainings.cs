using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTrainings : ReportTrainingsParticipation
    {
        public ReportMemberVsTrainings(DateTime datStart, DateTime datEnd, int idKurs)
            : base(datStart, datEnd)
        {
            CreateModel
            (
                bHide: false,
                new AxisTypeTraining(),
                new AxisTypeMember(),
                filter: tn => idKurs == Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID)
            );

            m_Info0.Text = AxisTypeCourse.GetDesc(idKurs, ' ');
            ReportFormPopulate();
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? "X" : " ";
    }
}
