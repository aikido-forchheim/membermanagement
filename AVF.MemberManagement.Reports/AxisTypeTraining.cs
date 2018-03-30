using System;
using System.Diagnostics;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeTraining : AxisType
    {
        public AxisTypeTraining(ReportDescriptor desc)
             : base(desc)
        {
            P_ActiveElementsOnly = true;
            P_MaxDbId = Globals.DatabaseWrapper.MaxTrainingNr();
            P_MinDbId = Globals.DatabaseWrapper.MinTrainingNr();
            HeaderStrings = new List<string> { "Training", "Zeit" };
        }

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.P_trainings.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => Globals.DatabaseWrapper.P_trainings[iModelIndex].Id;

        public override string MouseAxisEvent(int idTraining, bool action)
        {
            Debug.Assert(idTraining > 0);
            return action
               ? ReportMain.P_formMain.NewPanel(typeof(AxisTypeEmpty), typeof(AxisTypeMember), new TimeRange(Globals.DatabaseWrapper.TrainingFromId(idTraining).Termin), idTraining: idTraining)
               : $"Klicken für Details zum Training " + GetFullDesc(idTraining, '.');
        }

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;

        public override string GetFullDesc(int idTraining, char separator)
        {
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            return $"{training.Termin:dd}{separator}{training.Termin:MM}{separator}{training.Termin:yy}";
        }

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

                case 4:
                    return $"{training.Zeit:hh}:{training.Zeit:mm}";

                default:
                    Debug.Assert(false);
                    break;
            }
            return String.Empty;
        }
    }
}
