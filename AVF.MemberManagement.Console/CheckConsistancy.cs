using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Utilities;

namespace AVF.MemberManagement.Console
{
    class CheckConsistancy
    {
        public void Main(DatabaseWrapper db)
        {
            OutputFile ofile = new OutputFile( "Findings.txt", db );

            int iFinding = 0;

            // Trainer has to be participant of training

            DateTime dateStart = new DateTime(2017, 1, 1);

            foreach (Training training in db.AllTrainings())
            {
                int? trainer = training.Trainer;
                if ( ( training.Termin > dateStart ) && (trainer.HasValue && trainer > 0) )
                {
                    if (!db.HatTeilgenommen(training.Trainer, training))
                    {
                        ofile.Write($"Finding { ++iFinding }: Trainer hat nicht an Training teilgenommen. ");
                        ofile.WriteMitglied( trainer.Value );
                        ofile.Write( $" Training Nr. {training.Id} " );
                        ofile.WriteTraining(training, db.WeekDay(training.WochentagID));
                        ofile.WriteLine("");
                    }
                }
            }

            // Prüfungen:
            // Am Prüfungstag muss ein Training stattgefunden haben
            // Prüfling and Prüfer müssen Teilnehmer sein

            foreach (Pruefung pruefung in db.Pruefungen())
            {
                bool found = false;
                foreach (Training training in db.AllTrainings())
                {
                     if ( 
                          ( training.Termin == pruefung.Datum ) &&
                          ( db.HatTeilgenommen(pruefung.Pruefling, training )) &&
                          ( db.HatTeilgenommen(pruefung.Pruefer,   training ))
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
                    ofile.Write($"Finding { ++iFinding }: Kein passendes Training am Prüfungstag. ");
                    ofile.WritePruefung( pruefung );
                    ofile.Write( "Prüfling: " );
                    ofile.WriteMitglied( pruefung.Pruefling );
                    ofile.WriteLine();
                }
            }

            ofile.Close();
        }
    }
}
