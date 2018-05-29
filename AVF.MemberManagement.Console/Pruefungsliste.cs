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
                        oTarget.Write(examination.GraduationText());
                        oTarget.Write(examination.GraduationDate());
                        oTarget.Write(examination.Examinant());
                        oTarget.Write(examination.WaitTime());
                        oTarget.Write(examination.NrOfTrainings());
                        oTarget.WriteLine();
                    }
                    oTarget.WriteLine();
                }
            }
            oTarget.CloseAndReset2Console();
        }
    }
}
