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
                var pruefungen = new List<Pruefung>();

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
                        ofile.WritePruefung(pruefung, m_dbWrapper);
                        ofile.WriteLine();
                    }
                    ofile.WriteLine();
                }
            }
            ofile.Close();
        }
    }
}
