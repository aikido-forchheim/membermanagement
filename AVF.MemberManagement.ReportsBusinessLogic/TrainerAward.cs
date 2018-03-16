using System;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class TrainerAward : DatabaseWrapper
    {
        public decimal TrainingAward(int level, Training training, int trainerNr)
        {
            decimal decResult = 0;
            if (!training.VHS)
            {
                var stundensatz = Globals.DatabaseWrapper.P_stundensaetze.Single(s => (s.TrainerStufenID == level) && (s.TrainerNummer == trainerNr) && (s.Dauer == training.DauerMinuten));
                decResult = stundensatz.Betrag;
            }
            return decResult;
        }

        public decimal ExtraAmount(Training training, int trainerNr)
        {
            if (training.Kindertraining)
            {
                var zuschlag = Globals.DatabaseWrapper.P_zuschlagKinderTraining.Single(s => (s.Trainernummer == trainerNr) && (s.Dauer == training.DauerMinuten));
                return zuschlag.Betrag;
            }
            return 0;
        }

        public decimal CalcTravelExpenses(int? idMitglied, DateTime termin)
        {
            var wohnungsbezug = Globals.DatabaseWrapper.P_wohnungsbezug.Where(t => (t.MitgliedId == idMitglied) && ((t.Datum == null) || (t.Datum <= termin)));
            if (wohnungsbezug.Any())
            {
                var letzterUmzug = wohnungsbezug.Max(t => t.Datum);
                var wohnungId = wohnungsbezug.Single(s => (s.Datum == letzterUmzug)).WohnungId;
                var wohnung = Globals.DatabaseWrapper.P_wohnung.Single(s => (s.Id == wohnungId));
                if (wohnung.Fahrtstrecke.HasValue)
                {
                    Decimal Cents = wohnung.Fahrtstrecke.Value * 17;
                    return Decimal.Floor(Cents + 0.5M) / 100;
                }
            }
            return 0;
        }

        public int TrainerLevel(int? trainerId, DateTime termin)  // calculates level (Vereinstrainer, Lehrer ...) of a trainer at a given date
        {
            var ernennungen = Globals.DatabaseWrapper.P_trainerErnennungen.Where(t => (t.MitgliedID == trainerId) && ((t.Datum == null) || (t.Datum <= termin)));
            return ernennungen.Any() ? ernennungen.Max(t => t.StufeID) : 1;
        }

    }
}
