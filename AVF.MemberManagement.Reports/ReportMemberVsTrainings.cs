using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTrainings : ReportTrainingsParticipation
    {
        public ReportMemberVsTrainings(TimeRange timeRange, int idKurs)
            : base(timeRange)
        {
            CreateModel
            (
                new AxisTypeTraining(timeRange),
                new AxisTypeMember(timeRange),
                filter: tn => (idKurs == -1) ? true : (idKurs == Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID))
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
