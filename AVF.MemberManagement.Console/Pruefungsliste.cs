using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Pruefungsliste
    {
        internal async Task Main( DatabaseWrapper db )
        {
            OutputTarget oTarget = new OutputTarget("Pruefungsliste.txt", db );

            foreach ( Mitglied mitglied in db.m_mitglieder)
            {
                var examinations = Examinations.GetListOfExaminations(db, mitglied);
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
