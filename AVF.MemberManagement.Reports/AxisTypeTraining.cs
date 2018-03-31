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
               ? ReportMain.P_formMain.NewTrainingsParticipationPanel
                 (
                    defaultDesc: null,
                    xAxisType: typeof(AxisTypeEmpty),
                    yAxisType: typeof(AxisTypeMember), 
                    timeRange: new TimeRange(Globals.DatabaseWrapper.TrainingFromId(idTraining).Termin), 
                    idTraining: idTraining
                 )
               : $"Klicken für Details zu {HeaderStrings[0]} {idTraining}";
        }

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID;

        public override List<string> GetDescription(int idTraining)
            => GetDesc(idTraining);

        public static string GetDate(Training training, char separator)
            => $"{training.Termin:dd}" + separator + $"{training.Termin:MM}" + separator + $"{training.Termin:yy}";

        public static string GetTime(Training training)
            => $"{training.Zeit:hh}" + ':' + $"{training.Zeit:mm}";

        public static List<string> GetDesc(int idTraining)
        {
            List<string> list = new List<string>();
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
            list.Add($"{training.Termin:dd}");
            list.Add($"{training.Termin:MM}");
            list.Add($"{training.Termin:yy}");
            return list;
        }
    }
}
