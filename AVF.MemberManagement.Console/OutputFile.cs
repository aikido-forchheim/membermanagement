using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    internal class OutputFile : System.IO.StreamWriter
    {
        private DatabaseWrapper m_db;

        public OutputFile( string fileName, DatabaseWrapper db ) : base(fileName)
        {
            m_db = db;
            System.Console.WriteLine(fileName);
        }

        public void WriteAmount(decimal amount)
        {
            string s = (amount > 0) ? $" { amount,7 } € " : "";

            Write(s.PadRight(11));
        }

        public void WriteNumber( int i )
        {
            Write((i > 0) ? $"{ i, 7 }" : "       ");
        }

        public void WriteTraining(Training training, string weekDay)
        {
            Write($"{training.Termin:yyyy-MM-dd} {weekDay,-10} {training.Zeit:hh}:{training.Zeit:mm} {training.DauerMinuten,3} ");
        }

        public void WriteMitglied(Mitglied mitglied)
        {
            Write($"{ mitglied.Nachname,-12 } { mitglied.Vorname,-12 } ({ mitglied.Id,3 }) ");
        }

        public void WriteMitglied( int id )
        {
            WriteMitglied(m_db.MitgliedFromId(id));
        }

        public void WritePruefung( Pruefung pruefung )
        {
            Graduierung grad = m_db.GraduierungFromId(pruefung.GraduierungID);

            Write($"{grad.Bezeichnung} {pruefung.Datum:yyyy-MM-dd} Prüfer: ");
            if (pruefung.Pruefer > 0)
                WriteMitglied( pruefung.Pruefer );
            else
                Write($"{pruefung.Bemerkung}");
        }

        public void WriteMatrix( string [,] matrix )
        {
            for (int iRow = 0; iRow < matrix.GetLength(0); ++iRow)
            {
                for (int iCol = 0; iCol < matrix.GetLength(1); ++iCol)
                    Write(matrix[iRow, iCol]);
                WriteLine();
            }
        }
    }
}
