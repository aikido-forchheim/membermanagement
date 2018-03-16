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
                new AxisTypeTraining(P_datStart, P_datEnd),
                new AxisTypeMember(P_datStart, P_datEnd),
                filter: tn => (idKurs == -1) ? true : (idKurs == Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID))
            );

            if (idKurs >= 0)
            {
                int idTrainer = Globals.DatabaseWrapper.KursFromId(idKurs).Trainer;
                if (idTrainer > 0)
                    P_labelTrainer.Text = $"Trainer: {P_axisTypeMember.GetFullDesc(idTrainer)}";
            }

            SetupCourseSelector( idKurs );
            ReportFormPopulate();
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? "X" : " ";
    }
}
