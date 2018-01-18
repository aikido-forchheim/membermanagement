using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Trainingsteilnahme
    {
        private int WriteNrOfParticipations( DatabaseWrapper db, OutputFile ofile, List<Training> trainingsInTimeRange, int ? idKurs, int memberId )
        {
            var list = db.Filter( trainingsInTimeRange, idKurs );
            int iCount = db.AnzahleBesuche( memberId, list );
            ofile.Write((iCount > 0) ? $"{ iCount, 7 }" : "       ");
            return iCount;
        }

        internal async Task Main(DatabaseWrapper db, int iJahr)
        {
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);

            MemberVsCourse tp = new MemberVsCourse(db, datStart, datEnd);

            OutputFile ofile = new OutputFile( "Trainingsteilnahme.txt", db );

            ofile.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");
            ofile.WriteLine();

            string[,] matrix = tp.GetMatrix();

            ofile.WriteMatrix(matrix);

            ofile.Close();
        }
    }
}
