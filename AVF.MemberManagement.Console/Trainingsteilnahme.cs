using System;
using System.Collections.Generic;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Utilities;

namespace AVF.MemberManagement.Console
{
    class Trainingsteilnahme
    {
        public int[] KursDisplayOrder()
        {
            int[] displayOrder = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 0 };  // Should be stored in DB
            return displayOrder;
        }

        private int WriteNrOfParticipations( DatabaseWrapper db, OutputFile ofile, List<Training> trainingsInTimeRange, int ? idKurs, int memberId )
        {
            var list = db.Filter( trainingsInTimeRange, idKurs );
            int iCount = db.AnzahleBesuche( memberId, list );
            ofile.Write((iCount > 0) ? $"{ iCount, 7 }" : "       ");
            return iCount;
        }

        public void Main(DatabaseWrapper db)
        {
            int iJahr = 2017;
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);

            var trainingsInPeriod = db.TrainingsInPeriod( -1, datStart, datEnd );

            OutputFile ofile = new OutputFile( "Trainingsteilnahme.txt", db );

            ofile.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");
            ofile.WriteLine();

            ofile.Write("                       Mitglied  ");
            foreach (var kurs in db.Kurse())
            {
                string weekDay = db.WeekDay(kurs.WochentagID); 
                ofile.Write($"    { weekDay.Substring( 0, 2 ) } ");
            }
            ofile.WriteLine( " Lehrgänge" );

            ofile.Write("                         Nummer  ");
            foreach (var kurs in db.Kurse())
                ofile.Write($" {kurs.Zeit:hh}:{kurs.Zeit:mm} ");
            ofile.WriteLine(" Sondertr.  Summe");
            ofile.WriteLine();

            NrOfTrainings[] aNrOfTrainings = new NrOfTrainings[db.MaxMitgliedsNr() + 1];

            System.Console.WriteLine("acquire data");
            for (int memberId = 0; memberId < aNrOfTrainings.Length; memberId++)
            {
                System.Console.Write($"Member {memberId}\r");
                aNrOfTrainings[memberId] = 
                    new NrOfTrainings(
                                         memberId, 
                                         db.AnzahleBesuche( memberId, trainingsInPeriod )
                                     );
            }

            System.Console.WriteLine();
            System.Console.WriteLine("sorting");
            Array.Sort(aNrOfTrainings);
            System.Console.WriteLine("generate output");

            int iNrOfKurse = db.Kurse().Count();
            int[] aAnzahlBesucheInKurs = new int[iNrOfKurse + 1];

            for (int index = 0; index < aNrOfTrainings.Length; index++)
            {
                if (aNrOfTrainings[index].iCount > 0)
                {
                    int memberId = aNrOfTrainings[index].memberId;
                    System.Console.Write($"Member {memberId}\r");
                    ofile.WriteMitglied( memberId );

                    foreach (var kurs in db.Kurse())
                    {
                        aAnzahlBesucheInKurs[kurs.Id] += WriteNrOfParticipations( db, ofile, trainingsInPeriod, kurs.Id, memberId);
                    }

                    aAnzahlBesucheInKurs[iNrOfKurse] += WriteNrOfParticipations( db, ofile, trainingsInPeriod, null, memberId);

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
