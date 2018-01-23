using System;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Trainingsteilnahme
    {
        public enum ReportType { MemberVsTraining, MemberVsCourse, WeekVsCourse  };

        private static int GetUserInput( int iMax, string str )
        {
            int iResult;
            System.Console.WriteLine();
            do
            {
                System.Console.Write($"Please enter {str} (1..{iMax}) or 0 to exit and press ENTER: ");
                int.TryParse(System.Console.ReadLine(), out iResult);
            } while ((iResult < 0) || (iMax < iResult));

            return iResult;
        }

        internal static void Report(DatabaseWrapper db, int iJahr )
        {
            OutputTarget oTarget = new OutputTarget( db );

            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);

            var input = 0;
            while (input == 0)
            {
                System.Console.Clear();
                System.Console.WriteLine();
                System.Console.WriteLine("1: MemberVsTraining");
                System.Console.WriteLine("2: MemberVsCourse");
                System.Console.WriteLine("3: WeekVsCourse");

                input = GetUserInput(3, "number");

                switch (input)
                {
                    case 1:
                        {
                            MemberVsCourse tp = new MemberVsCourse(db, datStart, datEnd);

                            oTarget.SetOutputFile("TeilnahmeMitgliedKurs.txt");

                            oTarget.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");

                            oTarget.WriteLine();
                            oTarget.WriteMatrix(tp.GetMatrix());

                            oTarget.CloseAndReset2Console();
                        }
                        break;
                    case 2:
                        {
                            WeekVsCourse tp = new WeekVsCourse(db, datStart, datEnd, 144);

                            oTarget.SetOutputFile("TeilnahmeWocheKurs.txt");

                            oTarget.WriteLine($"Trainingsteilnahme    {datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");
                            oTarget.WriteLine();
                            oTarget.WriteMatrix(tp.GetMatrix());

                            oTarget.CloseAndReset2Console();
                        }
                        break;
                    case 3:
                        {
                            MemberVsTraining mt = new MemberVsTraining(db, datStart, datEnd);

                            System.Console.Clear();
                            input = GetUserInput(db.MaxKursNr(), "Course Id");
                             
                            if (input == 0)
                                break;

                            var kurs = db.KursFromId(input);
                            oTarget.SetOutputFile($"Kurs_{kurs.Id}.txt");

                            oTarget.WriteLine($"Kurs Nr. {kurs.Id}");
                            oTarget.WriteLine($"{db.WeekDay(kurs.WochentagID)} {kurs.Zeit}");
                            oTarget.WriteLine($"{datStart:dd.MM.yyyy} bis {datEnd:dd.MM.yyyy}");

                            oTarget.WriteMatrix(mt.GetMatrix(kurs.Id));
                            oTarget.CloseAndReset2Console();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
