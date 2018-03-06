using System;
using System.Diagnostics;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeTraining : AxisType
    {
        public AxisTypeTraining(DateTime datStart, DateTime datEnd)
             : base(datStart, datEnd)
        {
            P_ActiveElementsOnly = true;
            P_MaxDbId = Globals.DatabaseWrapper.MaxTrainingNr();
            P_MinDbId = Globals.DatabaseWrapper.MinTrainingNr();
            HeaderStrings = new List<string> { "Training" };
        }

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.P_trainings.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => Globals.DatabaseWrapper.P_trainings[iModelIndex].Id;

        public override string MouseAxisEvent(int idTraining, bool action)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            return action
               ? ReportMain.SwitchToPanel(new ReportTraining(training))
               : $"Klicken für Details zum Training " + GetFullDesc(training.Id, '.');
        }

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;

        public override string GetFullDesc(int idTraining, char separator)
            => GetDescription(idTraining, 1) + separator + GetDescription(idTraining, 2) + separator + GetDescription(idTraining, 3);

        public override string GetDescription(int idTraining, int iNr = 1)
        {
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);

            switch( iNr )
            {
                case 1:
                    return $"{training.Termin:dd}";

                case 2:
                    return $"{training.Termin:MM}";

                case 3:
                    return $"{training.Termin:yy}";

                default:
                    Debug.Assert(false);
                    break;
            }
            return String.Empty;
        }
    }
}
