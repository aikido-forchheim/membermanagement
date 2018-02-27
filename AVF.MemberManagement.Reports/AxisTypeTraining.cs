using System;
using System.Diagnostics;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeTraining : AxisType
    {
        public AxisTypeTraining(DateTime datStart, DateTime datEnd)
        { 
            P_ActiveElementsOnly = true;
            P_MaxDbId = Globals.DatabaseWrapper.MaxTrainingNr();
            P_MinDbId = Globals.DatabaseWrapper.MinTrainingNr();
    }

    public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idTraining, bool action)
            => action
               ? ReportTrainingsParticipation.Show(new ReportTraining(idTraining))
               : $"Klicken für Details zum Training " + GetDescription(idTraining, ' ');
        
        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;

        public override string GetDescription(int idTraining, char separator)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            return $"{training.Termin:dd}{separator}{training.Termin:MM}{separator}{training.Termin:yy}";
        }
    }
}
