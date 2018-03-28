using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTrainings : ReportTrainingsParticipation
    {
        public ReportMemberVsTrainings(TimeRange timeRange, int idMember, int idKurs)
            : base(timeRange, idMember, idKurs, Globals.ALL_TRAININGS)
        {
            CreateModel
            (
                new AxisTypeTraining(m_reportDescriptor),
                new AxisTypeMember(m_reportDescriptor)
            );

            if (idKurs >= 0)
            {
                int idTrainer = Globals.DatabaseWrapper.KursFromId(idKurs).Trainer;
                if (idTrainer > 0)
                    P_labelTrainer.Text = $"Trainer: {P_axisTypeMember.GetFullDesc(idTrainer)}";
            }

            ReportFormPopulate();
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? "X" : " ";
    }
}
