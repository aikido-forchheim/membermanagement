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

            TrainingParticipation tp = new TrainingParticipation(db, datStart, datEnd);

            OutputFile ofile = new OutputFile( "Trainingsteilnahme.txt", db );

            ofile.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");
            ofile.WriteLine();

            string[][] ColumnLabels = tp.GetColumnLabels();

            ofile.Write("                       Mitglied  ");
            foreach (var s in ColumnLabels[0])
                ofile.Write(s);
            ofile.WriteLine();

            ofile.Write("                         Nummer  ");
            foreach (var s in ColumnLabels[1])
                ofile.Write(s);
            ofile.WriteLine();

            ofile.WriteLine();

            ///////////////////////////////////////////////////

            {
                string[,] matrix = tp.GetMatrix();

                for (int iRow = 0; iRow < matrix.GetLength(0); ++iRow)
                {
                    for (int iCol = 0; iCol < matrix.GetLength(1); ++iCol)
                        ofile.Write(matrix[iRow,iCol]);
                    ofile.WriteLine();
                }

            }
            ofile.Close();
        }
    }
}
