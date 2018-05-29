using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Mitgliederbeitraege
    {
        internal async Task Main( )
        {
            OutputTarget oTarget = new OutputTarget( "Mitgliederbeitraege.txt" );

            decimal decHalbjahresSumme = 0;
            int iLfdNr = 0;

            foreach (Mitglied mitglied in Globals.DatabaseWrapper.P_mitglieder)
            {
                if (mitglied.Faktor > 0)
                {
                    int iProzentsatz = Globals.DatabaseWrapper.Familienrabatt(mitglied);
                    if ( iProzentsatz > 0 )
                    { 
                        decimal decStdJahresbeitrag  = Globals.DatabaseWrapper.BK(mitglied).Beitrag;
                        decimal decJahresbeitrag     = decStdJahresbeitrag * iProzentsatz / 100;
                        decimal decHalbjahresbeitrag = decJahresbeitrag / 2;
                        decHalbjahresSumme += decHalbjahresbeitrag;
                        oTarget.Write($"{ ++iLfdNr, 3}  ");
                        oTarget.WriteMitglied( mitglied) ;
                        oTarget.Write($"{ Globals.DatabaseWrapper.BK_Text(mitglied),3} ");
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
