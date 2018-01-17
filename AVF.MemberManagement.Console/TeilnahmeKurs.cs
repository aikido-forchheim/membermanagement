using System;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    class TeilnahmeKurs
    {
        public void Main( DatabaseWrapper db )
        {
            int iJahr = 2017;
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd   = new DateTime(iJahr, 12, 31);

            foreach (var kurs in db.Kurse())
            {
                OutputFile ofile = new OutputFile($"Kurs_{kurs.Id}.txt", db );

                ofile.WriteLine($"Kurs Nr. {kurs.Id}");
                ofile.WriteLine($"{db.WeekDay(kurs.WochentagID)} {kurs.Zeit}");
                ofile.WriteLine($"{datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");

                var relevantTrainings = db.TrainingsInPeriod( kurs.Id, datStart, datEnd );

                ofile.Write("                          Monat ");
                foreach (var training in relevantTrainings)
                    ofile.Write($"{training.Termin:MM} ");
                ofile.WriteLine();

                ofile.Write("                            Tag ");
                foreach (var training in relevantTrainings)
                    ofile.Write($"{training.Termin:dd} ");
                ofile.WriteLine("Summe");

                foreach (var mitglied in db.Mitglieder())
                {
                    if (db.HatTeilgenommen(mitglied.Id, relevantTrainings))
                    {
                        int iCount = 0;
                        ofile.WriteMitglied(mitglied);
                        foreach (var training in relevantTrainings)
                        {
                            if (db.HatTeilgenommen(mitglied.Id, training))
                            {
                                ofile.Write(" X ");
                                ++iCount;
                            }
                            else
                                ofile.Write("   ");
                        }
                        ofile.WriteLine($"{ iCount,3 }");
                    }
                }
                ofile.Close();
            }
        }
    }
}
