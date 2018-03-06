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
                filter: tn => idKurs == Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID)
            );

            P_Info0.Text = $"Kurs Nr. {idKurs}: " + new AxisTypeCourse(P_datStart, P_datEnd).GetDescription(idKurs);
            P_Info1.Text = $"Trainer: {P_axisTypeMember.GetFullDesc(Globals.DatabaseWrapper.KursFromId(idKurs).Trainer)}";
            ReportFormPopulate();
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? "X" : " ";
    }
}
