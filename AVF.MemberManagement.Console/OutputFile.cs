
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Console
{
    internal class OutputTarget // : System.IO.StreamWriter
    {
        private System.IO.StreamWriter m_ofile;
        private System.IO.TextWriter m_ConsoleOut;

        private void initialize()
        {
            m_ConsoleOut = System.Console.Out;
        }

        public OutputTarget()
        {
            initialize( );
        }

        public OutputTarget(string fileName )
        {
            m_ConsoleOut = System.Console.Out;
            SetOutputFile(fileName);
        }

        public void SetOutputFile(string fileName) // : base(fileName)
        {
            m_ofile = new System.IO.StreamWriter(fileName);
            System.Console.WriteLine(fileName);
            System.Console.SetOut(m_ofile);
        }

        public void CloseAndReset2Console()
        {
            m_ofile.Close();
            m_ofile = null;
            System.Console.SetOut(m_ConsoleOut);
        }

        public void Write(string str)
        {
            System.Console.Write(str);
        }

        public void WriteLine()
        {
            System.Console.WriteLine();
        }

        public void WriteLine(string str)
        {
            System.Console.WriteLine(str);
        }

        public void WriteAmount(decimal amount)
        {
            string s = (amount > 0) ? $" { amount,7 } € " : "";

            System.Console.Write(s.PadRight(11));
        }

        public void WriteNumber( int i )
        {
            System.Console.Write( Utilities.FormatNumber(i) );
        }

        public void WriteTraining(Training training, string weekDay)
        {
            System.Console.Write(Utilities.FormatTraining(training, weekDay));
        }

        public void WriteTraining(Training training, int id )
        {
            WriteTraining(training, Globals.DatabaseWrapper.WeekDay(id));
        }

        public void WriteMitglied(Mitglied mitglied)
        {
            System.Console.Write(Utilities.FormatMitglied(mitglied));
        }

        public void WriteMitglied( int id )
        {
            WriteMitglied(Globals.DatabaseWrapper.MitgliedFromId(id));
        }

        public void WritePruefung( Pruefung pruefung )
        {
            Graduierung grad = Globals.DatabaseWrapper.GraduierungFromId(pruefung.GraduierungID);

            System.Console.Write($"{grad.Bezeichnung} {pruefung.Datum:dd-MM-yyyy} Prüfer: ");
            if (pruefung.Pruefer > 0)
            {
                Mitglied pruefer = Globals.DatabaseWrapper.MitgliedFromId(pruefung.Pruefer);
                System.Console.Write( $"{ pruefer.Nachname }, { pruefer.Vorname } ({ pruefer.Id }) " );
            }
            else
                System.Console.Write($"{pruefung.Bemerkung}");
        }

        public void WriteMatrix( string [,] matrix )
        {
            for (int iRow = 0; iRow < matrix.GetLength(0); ++iRow)
            {
                for (int iCol = 0; iCol < matrix.GetLength(1); ++iCol)
                    System.Console.Write(matrix[iRow, iCol]);
                System.Console.WriteLine();
            }
        }
    }
}
