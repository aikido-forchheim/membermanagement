using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Console
{
    class Mitgliederbeitraege
    {
        private DatabaseWrapper m_dbWrapper;

        public async Task Main()
        {
            OutputFile ofile = new OutputFile("Mitgliederbeitraege.txt");

            m_dbWrapper = new DatabaseWrapper();

            await m_dbWrapper.ReadTables();

            decimal decHalbjahresSumme = 0;
            int iLfdNr = 0;

            foreach (Mitglied mitglied in m_dbWrapper.Mitglieder())
            {
                if (mitglied.Faktor > 0)
                {
                    int iProzentsatz = m_dbWrapper.Familienrabatt(mitglied);
                    if ( iProzentsatz > 0 )
                    { 
                        decimal decStdJahresbeitrag  = m_dbWrapper.BK(mitglied).Beitrag;
                        decimal decJahresbeitrag     = decStdJahresbeitrag * iProzentsatz / 100;
                        decimal decHalbjahresbeitrag = decJahresbeitrag / 2;
                        decHalbjahresSumme += decHalbjahresbeitrag;
                        ofile.Write($"{ ++iLfdNr, 3}  ");
                        ofile.WriteMitglied( mitglied) ;
                        ofile.Write($"{ m_dbWrapper.BK_Text(mitglied),3} ");
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
