using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Console
{
    internal class OutputFile : System.IO.StreamWriter
    {
        public OutputFile(string fileName) : base(fileName)
        {
        }

        public void WriteAmount(string prefix, decimal amount)
        {
            string s = $"{prefix} { amount,7 } € ";

            if (amount > 0)
            {
                Write(s);
            }
            else
            {
                for (int i = 0; i < s.Length; ++i)
                    Write($" ");
            }
        }

        public void WriteTraining(Training training, string weekDay)
        {
            Write($"{training.Termin:yyyy-MM-dd} {weekDay,-10} {training.Zeit:hh}:{training.Zeit:mm}, {training.DauerMinuten,3} min, ");
        }

        public void WriteMitglied(Mitglied mitglied)
        {
            Write($"{ mitglied.Nachname,-12 } { mitglied.Vorname,-12 } ({ mitglied.Id,3 }) ");
        }
    }
}
