using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Mitgliederbeitraege
    {
        internal async Task Main( DatabaseWrapper db )
        {
            OutputTarget oTarget = new OutputTarget( "Mitgliederbeitraege.txt", db );

            decimal decHalbjahresSumme = 0;
            int iLfdNr = 0;

            foreach (Mitglied mitglied in db.m_mitglieder)
            {
                if (mitglied.Faktor > 0)
                {
                    int iProzentsatz = db.Familienrabatt(mitglied);
                    if ( iProzentsatz > 0 )
                    { 
                        decimal decStdJahresbeitrag  = db.BK(mitglied).Beitrag;
                        decimal decJahresbeitrag     = decStdJahresbeitrag * iProzentsatz / 100;
                        decimal decHalbjahresbeitrag = decJahresbeitrag / 2;
                        decHalbjahresSumme += decHalbjahresbeitrag;
                        oTarget.Write($"{ ++iLfdNr, 3}  ");
                        oTarget.WriteMitglied( mitglied) ;
                        oTarget.Write($"{ db.BK_Text(mitglied),3} ");
                        oTarget.WriteAmount( decJahresbeitrag );
                        oTarget.Write($"{ mitglied.Familienmitglied } ");
                        oTarget.WriteAmount( decHalbjahresbeitrag );
                        oTarget.WriteLine();
                    }
                }
            }

            oTarget.WriteLine();
            oTarget.WriteLine("Halbjahressumme: ");
            oTarget.WriteAmount( decHalbjahresSumme );

            oTarget.CloseAndReset2Console();
        }
    }
}
