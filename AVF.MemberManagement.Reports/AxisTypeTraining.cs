using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeTraining : AxisType
    {
        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idTraining, bool action)
            => action
               ? ReportTrainingsParticipation.Show(new ReportTraining(idTraining))
               : $"Klicken für Details zum Training " + Globals.GetTrainingDescription(idTraining);
        
        public override int P_MaxDbId { get; } = Globals.DatabaseWrapper.MaxTrainingNr();

        public override int P_MinDbId { get; } = Globals.DatabaseWrapper.MinTrainingNr();

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;
    }
}
