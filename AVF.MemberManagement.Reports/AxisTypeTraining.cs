using System;
using System.Diagnostics;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeTraining : AxisType
    {
        public AxisTypeTraining()
        { 
            P_ActiveElementsOnly = true;
            P_MaxDbId = Globals.DatabaseWrapper.MaxTrainingNr();
            P_MinDbId = Globals.DatabaseWrapper.MinTrainingNr();
            HeaderStrings = new List<string> { "Training" };
        }

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.m_trainings.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => Globals.DatabaseWrapper.m_trainings[iModelIndex].Id;

        public override string MouseAxisEvent(DateTime datStart, DateTime datEnd, int idTraining, bool action)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            return action
               ? ReportMain.SwitchToPanel(new ReportTraining(training))
               : $"Klicken für Details zum Training " + GetDescription(idTraining);
        }

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;

        public override string GetDescription(int idTraining, int iNr = 1)
            => GetDesc(idTraining, iNr);

        public static string GetDesc(int idTraining, int iNr = 1)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            return $"{training.Termin:dd}\n{training.Termin:MM}\n{training.Termin:yy}";
        }
    }
}
