using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.BusinessLogic
{
    public static class Utilities
    {
        public static string FormatNumber(int i)
            => (i > 0) ? $"{ i,7 }" : "       ";

        public static string FormatMitglied(Mitglied mitglied)
            => $"{ mitglied.Nachname,-12 } { mitglied.Vorname,-12 } ({ mitglied.Id,3 }) ";

        public static string FormatTraining(Training training, string weekDay)
            => $"{training.Termin:yyyy-MM-dd} {weekDay,-10} {training.Zeit:hh}:{training.Zeit:mm} {training.DauerMinuten,3} ";
    }
}
