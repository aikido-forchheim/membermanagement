using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Console
{
    class Pruefungsliste
    {
        private DatabaseWrapper m_dbWrapper;

        public async Task Main()
        {
            OutputFile ofile = new OutputFile("Pruefungsliste.txt");

            m_dbWrapper = new DatabaseWrapper();

            await m_dbWrapper.ReadTables();

            foreach ( Mitglied mitglied in m_dbWrapper.Mitglieder() )
            {
                List<Pruefung> pruefungen = new List<Pruefung>();

                foreach ( Pruefung pruefung in m_dbWrapper.Pruefungen() )
                {
                    if ( pruefung.Pruefling == mitglied.Id )
                    {
                        pruefungen.Add(pruefung);
                    }
                }

                if (pruefungen.Count > 0)
                {
                    ofile.WriteMitglied(mitglied);
                    ofile.WriteLine();
                    foreach (Pruefung pruefung in pruefungen)
                    {
                        Graduierung grad = m_dbWrapper.GraduierungFromId(pruefung.GraduierungID);

                        ofile.Write($"{grad.Bezeichnung} {pruefung.Datum:yyyy-MM-dd} Prüfer: ");
                        if (pruefung.Pruefer > 0)
                            ofile.WriteMitglied(m_dbWrapper.MitgliedFromId(pruefung.Pruefer));
                        else
                            ofile.Write( $"{pruefung.Bemerkung}" );
                        ofile.WriteLine();
                    }
                    ofile.WriteLine();
                }
            }
            ofile.Close();
        }
    }
}
