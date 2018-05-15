using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Pruefungsliste
    {
        internal async Task Main()
        {
            OutputTarget oTarget = new OutputTarget("Pruefungsliste.txt");

            foreach (Mitglied member in Globals.DatabaseWrapper.CurrentMembers())
            {
                var examinations = new Examinations().GetSortedListOfExaminations(member);
                if ((examinations.Count > 0)&&(examinations[0].P_exam.GraduierungID > 7))
                {
                    DateTime datValidData = Globals.DatabaseWrapper.GetStartValidData();
                    oTarget.WriteMitglied(member);
                    oTarget.WriteLine();
                    foreach (var examination in examinations)
                    {
                        oTarget.WritePruefung(examination.P_exam);

                        if (examination.P_monthsSinceLastExam > 0)
                            oTarget.Write($" {examination.P_monthsSinceLastExam,3}({examination.P_waitTimeMonths})");
                        else
                            oTarget.Write("        ");

                        if (examination.P_exam.Datum < datValidData)
                            oTarget.Write(" ????");
                        else
                        {
                            oTarget.Write((examination.P_dateLastExam < datValidData) ? " >" : "  ");
                            oTarget.Write($"{ examination.P_nrOfTrainingsSinceLastExam, 3 }");
                        }
                        oTarget.Write($" ({examination.P_nrOfTrainingsNeeded})");
                        oTarget.WriteLine();
                    }
                    oTarget.WriteLine();
                }
            }
            oTarget.CloseAndReset2Console();
        }
    }
}
