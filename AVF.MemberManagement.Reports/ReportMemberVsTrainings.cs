using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTrainings : ReportTrainingsParticipation
    {
        public ReportMemberVsTrainings(DateTime datStart, DateTime datEnd, int idKurs)
        {
            CreateModel
            (
                datStart, datEnd,
                new HorizontalAxisTrainings(datStart, datEnd),
                new VerticalAxisMembers(),
                filter: tn => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID) == idKurs
            );

            m_labelReportName.Text = "Kursteilnahme";
            m_Info0.Text = AxisTypeCourse.GetDesc(idKurs, ' ');
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? "X" : " ";
    }
}
