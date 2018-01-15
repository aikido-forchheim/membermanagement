using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Utilities;

namespace AVF.MemberManagement.Console
{
    class Pruefungsliste
    {
        public void Main( DatabaseWrapper db )
        {
            OutputFile ofile = new OutputFile("Pruefungsliste.txt", db );

            foreach ( Mitglied mitglied in db.Mitglieder() )
            {
                var pruefungen = new List<Pruefung>();

                foreach ( Pruefung pruefung in db.Pruefungen() )
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
                        ofile.WritePruefung( pruefung );
                        ofile.WriteLine();
                    }
                    ofile.WriteLine();
                }
            }
            ofile.Close();
        }
    }
}
