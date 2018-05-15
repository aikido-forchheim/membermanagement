using System;
using System.Linq;
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
                if (examinations.Count > 0)
                {
                    DateTime datValidData = Globals.DatabaseWrapper.GetStartValidData();
                    oTarget.WriteMitglied(member);
                    oTarget.WriteLine();
                    foreach (var examination in examinations)
                    {
                        if (examination.P_exam == null)
                        {
                            oTarget.Write("Eintritt: ");
                            oTarget.WriteDatum(examination.P_range.P_datStart);
                        }
                        else
                        {
                            oTarget.WritePruefung(examination.P_exam);
                        }

                        if (examination.P_monthsTilNextExam > 0)
                            oTarget.Write($" {examination.P_monthsTilNextExam,3}({examination.P_waitTimeMonths})");
                        else
                            oTarget.Write("        ");

                        if (examination.P_range.P_datEnd < datValidData)
                            oTarget.Write(" ????");
                        else
                        {
                            oTarget.Write((examination.P_range.P_datStart < datValidData) ? " >" : "  ");
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
