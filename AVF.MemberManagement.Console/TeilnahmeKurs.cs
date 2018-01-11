using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Console
{
    class TeilnahmeKurs
    {
        private DatabaseWrapper m_dbWrapper;

        public async Task Main()
        {
            m_dbWrapper = new DatabaseWrapper();

            await m_dbWrapper.ReadTables();

            int iJahr = 2017;
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd   = new DateTime(iJahr, 12, 31);

            var trainingsInTimeRange = m_dbWrapper.AllTrainings().Where(training => training.Termin > datStart && training.Termin < datEnd).ToList();

            foreach (var kurs in m_dbWrapper.Kurse() )
            {
                string oFileName = $"Kurs_{kurs.Id}.txt";
                OutputFile ofile = new OutputFile( oFileName );
                System.Console.WriteLine( oFileName );
                ofile.WriteLine($"Kurs Nr. {kurs.Id}");
                ofile.WriteLine($"{m_dbWrapper.WeekDay(kurs.WochentagID)} {kurs.Zeit}");
                ofile.WriteLine($"{datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");

                var relevantTrainings = trainingsInTimeRange.Where(training => training.KursID == kurs.Id).ToList();
                relevantTrainings = relevantTrainings.OrderBy(x => x.Termin).ToList();

                ofile.Write("                          Monat ");
                foreach (var training in relevantTrainings)
                {
                    ofile.Write($"{training.Termin:MM} ");
                }
                ofile.WriteLine();

                ofile.Write("                            Tag ");
                foreach (var training in relevantTrainings)
                {
                    ofile.Write($"{training.Termin:dd} ");
                }
                ofile.WriteLine( "Summe" );

                foreach (var mitglied in m_dbWrapper.Mitglieder())
                {
                    if (m_dbWrapper.HatTeilgenommen( mitglied.Id, relevantTrainings ) )
                    {
                        int iCount = 0;
                        ofile.WriteMitglied(mitglied);

                        foreach (var training in relevantTrainings)
                        {
                            if (m_dbWrapper.HatTeilgenommen(mitglied.Id, training))
                            {
                                ofile.Write(" X ");
                                ++iCount;
                            }
                            else
                                ofile.Write("   ");
                        }
                        ofile.WriteLine( $"{ iCount, 3 }");
                    }
                }
                ofile.Close();
            }
        }
    }
}
