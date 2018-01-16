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

            int[,] MatrixKursMitglieder = db.TrainingParticipation( datStart, datEnd );


            OutputFile ofile = new OutputFile( "Trainingsteilnahme.txt", db );

            ofile.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");
            ofile.WriteLine();

            ofile.Write("                       Mitglied  ");
            foreach (var kurs in db.Kurse())
            {
                string weekDay = db.WeekDay(kurs.WochentagID).Substring(0, 2); 
                ofile.Write($"    { weekDay } ");
            }
            ofile.WriteLine( " Lehrgänge" );

            ofile.Write("                         Nummer  ");
            foreach (var kurs in db.Kurse())
                ofile.Write($" {kurs.Zeit:hh}:{kurs.Zeit:mm} ");
            ofile.WriteLine(" Sondertr.  Summe");

            ofile.WriteLine();

            ///////////////////////////////////////////////////

            {
                int iNrOfRows = MatrixKursMitglieder.GetLength(0);
                int iNrOfCols = MatrixKursMitglieder.GetLength(1);

                int[] RowSum = new int[iNrOfRows];
                int[] ColSum = new int[iNrOfCols];

                for (int iRow = 0; iRow < RowSum.Length; ++iRow)
                    for (int iCol = 0; iCol < ColSum.Length; ++iCol)
                    {
                        RowSum[iRow] += MatrixKursMitglieder[iRow, iCol];
                        ColSum[iCol] += MatrixKursMitglieder[iRow, iCol];
                    }

                for (int iRow = 0; iRow < iNrOfRows; ++iRow)
                {
                    if (RowSum[iRow] > 0)
                    {
                        ofile.WriteMitglied(iRow);
                        for (int iCol = 0; iCol < iNrOfCols; ++iCol)
                            ofile.WriteNumber(MatrixKursMitglieder[iRow, iCol]);
                        ofile.WriteNumber(RowSum[iRow]);
                        ofile.WriteLine();
                    }
                }

                ofile.Write("                     Insgesamt  ");
                for (int iCol = 0; iCol < iNrOfCols; ++iCol)
                    ofile.WriteNumber(ColSum[iCol]);
                ofile.WriteLine();
            }

            ofile.Close();
        }
    }
}
