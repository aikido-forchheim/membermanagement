using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTrainings : ReportTrainingsParticipation
    {
        public ReportMemberVsTrainings(TimeRange timeRange, int idMember, int idKurs)
            : base(timeRange, idMember, idKurs)
        {
            CreateModel
            (
                new AxisTypeTraining(m_reportDescriptor),
                new AxisTypeMember(m_reportDescriptor)
            );

            ReportFormPopulate();
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? "X" : " ";
    }
}
