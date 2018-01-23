using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Pruefungsliste
    {
        internal async Task Main( DatabaseWrapper db )
        {
            OutputTarget oTarget = new OutputTarget("Pruefungsliste.txt", db );

            foreach ( Mitglied mitglied in db.m_mitglieder)
            {
                var pruefungen = new List<Pruefung>();

                foreach ( Pruefung pruefung in db.m_pruefung )
                {
                    if ( pruefung.Pruefling == mitglied.Id )
                    {
                        pruefungen.Add(pruefung);
                    }
                }

                if (pruefungen.Count > 0)
                {
                    oTarget.WriteMitglied(mitglied);
                    oTarget.WriteLine();
                    foreach (Pruefung pruefung in pruefungen)
                    {
                        oTarget.WritePruefung( pruefung );
                        oTarget.WriteLine();
                    }
                    oTarget.WriteLine();
                }
            }
            oTarget.CloseAndReset2Console();
        }
    }
}
