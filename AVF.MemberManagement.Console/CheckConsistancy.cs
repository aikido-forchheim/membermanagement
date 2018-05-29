using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Console
{
    class CheckConsistancy
    {
        internal async Task Main()
        {
            OutputTarget oTarget = new OutputTarget( "Findings.txt" );

            int iFinding = 0;

            // Trainer muss auch als Teilnehmer gelistet sein

            DateTime dateStart = new DateTime(2017, 1, 1);

            foreach (Training training in Globals.DatabaseWrapper.P_trainings)
            {
                int? trainer = training.Trainer;
                if ( ( training.Termin > dateStart ) && (trainer.HasValue && trainer > 0) )
                {
                    if (!Globals.DatabaseWrapper.HatTeilgenommen(training.Trainer, training))
                    {
                        oTarget.Write($"Finding { ++iFinding }: Trainer hat nicht an Training teilgenommen. ");
                        oTarget.WriteMitglied( trainer.Value );
                        oTarget.Write( $" Training Nr. {training.Id} " );
                        oTarget.WriteTraining(training, training.WochentagID);
                        oTarget.WriteLine("");
                    }
                }
            }

            // Prüfungen:
            // Am Prüfungstag muss ein Training stattgefunden haben
            // Prüfling and Prüfer müssen Teilnehmer sein

            foreach (Pruefung pruefung in Globals.DatabaseWrapper.P_pruefung)
            {  
                bool found = false;
                foreach (Training training in Globals.DatabaseWrapper.P_trainings)
                {
                     if (
                          ( training.Termin == pruefung.Datum ) &&
                          (Globals.DatabaseWrapper.HatTeilgenommen(pruefung.Pruefling, training )) &&
                          (Globals.DatabaseWrapper.HatTeilgenommen(pruefung.Pruefer,   training ))
                       )
                    {
                        found = true;
                        break;
                    }
                }

                if ( 
                      ! found && 
                      (pruefung.Datum >= dateStart) &&   // Daten sind erst ab 2017 zuverlässig
                      (pruefung.Pruefer != 374)          // Kindertrainings nicht vollständig erfasst
                   )
                {
                    oTarget.Write($"Finding { ++iFinding }: Kein passendes Training am Prüfungstag. ");
                    oTarget.WritePruefung( pruefung );
                    oTarget.Write( "Prüfling: " );
                    oTarget.WriteMitglied( pruefung.Pruefling );
                    oTarget.WriteLine();
                }
            }

            oTarget.CloseAndReset2Console();
        }
    }
}
