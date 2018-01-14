using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Utilities;

namespace AVF.MemberManagement.Console
{
    class Mitgliederbeitraege
    {
        public void Main( DatabaseWrapper db )
        {
            OutputFile ofile = new OutputFile( "Mitgliederbeitraege.txt", db );

            decimal decHalbjahresSumme = 0;
            int iLfdNr = 0;

            foreach (Mitglied mitglied in db.Mitglieder())
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
                        ofile.Write($"{ ++iLfdNr, 3}  ");
                        ofile.WriteMitglied( mitglied) ;
                        ofile.Write($"{ db.BK_Text(mitglied),3} ");
                        ofile.WriteAmount( decJahresbeitrag );
                        ofile.Write($"{ mitglied.Familienmitglied } ");
                        ofile.WriteAmount( decHalbjahresbeitrag );
                        ofile.WriteLine();
                    }
                }
            }

            ofile.WriteLine();
            ofile.WriteLine("Halbjahressumme: ");
            ofile.WriteAmount( decHalbjahresSumme );

            ofile.Close();
        }
    }
}
