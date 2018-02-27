using System;
using System.Diagnostics;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeTraining : AxisType
    {
        public AxisTypeTraining()
        {
            P_ActiveElementsOnly = true;
        }

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idTraining, bool action)
            => action
               ? ReportTrainingsParticipation.Show(new ReportTraining(idTraining))
               : $"Klicken für Details zum Training " + GetDescription(idTraining);
        
        public override int P_MaxDbId { get; } = Globals.DatabaseWrapper.MaxTrainingNr();

        public override int P_MinDbId { get; } = Globals.DatabaseWrapper.MinTrainingNr();

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;

        public override string GetDescription(int idTraining)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            return $"{training.Termin:dd}\n{training.Termin:MM}";
        }
    }
}
