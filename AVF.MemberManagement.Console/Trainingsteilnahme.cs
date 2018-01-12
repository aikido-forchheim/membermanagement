using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Console
{
    class Trainingsteilnahme
    {
        private DatabaseWrapper m_dbWrapper;

        private class NrOfTrainings : IComparable<NrOfTrainings>
        {
            public int memberId;
            public int iCount;

            public NrOfTrainings( int m, int i )
            {
                memberId = m;
                iCount = i;
            }

            public int CompareTo(NrOfTrainings other)
            {
                return other.iCount - iCount;
            }
        };

        private int WriteNrOfParticipations( OutputFile ofile, List<Training> trainingsInTimeRange, int ? kursId, int memberId )
        {
            var list   = trainingsInTimeRange.Where(training => training.KursID == kursId).ToList();
            int iCount = m_dbWrapper.AnzahleBesuche(memberId, list);
            ofile.Write((iCount > 0) ? $"{ iCount, 7 }" : "       ");
            return iCount;
        }

        public async Task Main()
        {
            m_dbWrapper = new DatabaseWrapper();

            System.Console.WriteLine("read database");
            await m_dbWrapper.ReadTables();

            int iJahr = 2017;
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);

            var trainingsInPeriod = m_dbWrapper.TrainingsInPeriod( datStart, datEnd );

            OutputFile ofile = new OutputFile( "Trainingsteilnahme.txt" );

            ofile.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");
            ofile.WriteLine();

            ofile.Write("                       Mitglied  ");
            foreach (var kurs in m_dbWrapper.Kurse())
            {
                string weekDay = m_dbWrapper.WeekDay(kurs.WochentagID); 
                ofile.Write($"    { weekDay.Substring( 0, 2 ) } ");
            }
            ofile.WriteLine( " Lehrgänge" );

            ofile.Write("                         Nummer  ");
            foreach (var kurs in m_dbWrapper.Kurse())
                ofile.Write($" {kurs.Zeit:hh}:{kurs.Zeit:mm} ");
            ofile.WriteLine(" Sondertr.  Summe");
            ofile.WriteLine();

            NrOfTrainings[] aNrOfTrainings = new NrOfTrainings[m_dbWrapper.MaxMitgliedsNr() + 1];

            System.Console.WriteLine("acquire data");
            for (int memberId = 0; memberId < aNrOfTrainings.Length; memberId++)
            {
                System.Console.Write($"Member {memberId}\r");
                aNrOfTrainings[memberId] = 
                    new NrOfTrainings(
                                         memberId, 
                                         m_dbWrapper.AnzahleBesuche( memberId, trainingsInPeriod )
                                     );
            }

            System.Console.WriteLine();
            System.Console.WriteLine("sorting");
            Array.Sort(aNrOfTrainings);
            System.Console.WriteLine("generate output");

            int iNrOfKurse = m_dbWrapper.Kurse().Count();
            int[] aAnzahlBesucheInKurs = new int[iNrOfKurse + 1];
            for (int i = 0; i <= iNrOfKurse; ++i)
                aAnzahlBesucheInKurs[i] = 0;

            for (int index = 0; index < aNrOfTrainings.Length; index++)
            {
                if (aNrOfTrainings[index].iCount > 0)
                {
                    int memberId = aNrOfTrainings[index].memberId;
                    System.Console.Write($"Member {memberId}\r");
                    ofile.WriteMitglied(memberId, m_dbWrapper);

                    foreach (var kurs in m_dbWrapper.Kurse())
                    {
                        aAnzahlBesucheInKurs[kurs.Id] += WriteNrOfParticipations(ofile, trainingsInPeriod, kurs.Id, memberId);
                    }

                    aAnzahlBesucheInKurs[iNrOfKurse] += WriteNrOfParticipations(ofile, trainingsInPeriod, null, memberId);

                    ofile.WriteLine($"{ aNrOfTrainings[index].iCount, 11 }");
                }
            }

            ofile.Write("                     Insgesamt  ");
            for (int i = 0; i <= iNrOfKurse; ++i)
                ofile.Write( $"{ aAnzahlBesucheInKurs[i], 7 }" );

            System.Console.WriteLine();
            System.Console.WriteLine( "ready" );
            ofile.Close();
        }
    }
}
