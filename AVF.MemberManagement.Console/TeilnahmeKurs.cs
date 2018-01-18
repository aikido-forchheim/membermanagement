using System;
using System.Threading.Tasks;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    class TeilnahmeKurs
    {
        internal async Task Main( DatabaseWrapper db )
        {
            int iJahr = 2017;
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd   = new DateTime(iJahr, 12, 31);

            MemberVsTraining mt = new MemberVsTraining(db, datStart, datEnd);

            foreach (var kurs in db.Kurse())
            {
                OutputFile ofile = new OutputFile($"Kurs_{kurs.Id}.txt", db );

                ofile.WriteLine($"Kurs Nr. {kurs.Id}");
                ofile.WriteLine($"{db.WeekDay(kurs.WochentagID)} {kurs.Zeit}");
                ofile.WriteLine($"{datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");

                ofile.WriteMatrix(mt.GetMatrix(kurs.Id));
                ofile.Close();
            }
        }
    }
}
