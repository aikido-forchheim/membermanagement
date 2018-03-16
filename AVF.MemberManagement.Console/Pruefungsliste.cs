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

            foreach ( Mitglied mitglied in Globals.DatabaseWrapper.P_mitglieder)
            {
                var examinations = Examinations.GetListOfExaminations(mitglied);
                if (examinations.Length > 0)
                {
                    oTarget.WriteMitglied(mitglied);
                    oTarget.WriteLine();
                    foreach (Examination examination in examinations)
                    {
                        oTarget.WritePruefung(examination.exam);
                        oTarget.WriteLine();
                    }
                    oTarget.WriteLine();
                }
            }
            oTarget.CloseAndReset2Console();
        }
    }
}
