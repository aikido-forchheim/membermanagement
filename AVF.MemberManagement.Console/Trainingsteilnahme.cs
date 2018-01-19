using System;
using System.Threading.Tasks;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Trainingsteilnahme
    {
        public enum ReportType { MemberVsTraining, MemberVsCourse, WeekVsCourse  };

        internal static void Report(DatabaseWrapper db, int iJahr, ReportType type )
        {
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);

            switch ( type )
            {
                case ReportType.MemberVsCourse:
                    {
                        MemberVsCourse tp = new MemberVsCourse(db, datStart, datEnd);

                        OutputFile ofile = new OutputFile("TeilnahmeMitgliedKurs.txt", db);

                        ofile.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");

                        ofile.WriteLine();
                        ofile.WriteMatrix(tp.GetMatrix());

                        ofile.Close();
                    }
                    break;

                case ReportType.WeekVsCourse:
                    {
                        WeekVsCourse tp = new WeekVsCourse(db, datStart, datEnd, 144);

                        OutputFile ofile = new OutputFile("TeilnahmeWocheKurs.txt", db);

                        ofile.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");
                        ofile.WriteLine();
                        ofile.WriteMatrix(tp.GetMatrix());

                        ofile.Close();
                    }
                    break;

                case ReportType.MemberVsTraining:
                    {
                        MemberVsTraining mt = new MemberVsTraining(db, datStart, datEnd);

                        foreach (var kurs in db.Kurse())
                        {
                            OutputFile ofile = new OutputFile($"Kurs_{kurs.Id}.txt", db);

                            ofile.WriteLine($"Kurs Nr. {kurs.Id}");
                            ofile.WriteLine($"{db.WeekDay(kurs.WochentagID)} {kurs.Zeit}");
                            ofile.WriteLine($"{datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");

                            ofile.WriteMatrix(mt.GetMatrix(kurs.Id));
                            ofile.Close();
                        }
                    }
                    break;
            }
        }
    }
}
