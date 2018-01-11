using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Console
{
    class CheckConsistancy
    {
        private DatabaseWrapper m_dbWrapper;

        public async Task Main()
        {
            OutputFile ofile = new OutputFile("Findings.txt");

            m_dbWrapper = new DatabaseWrapper();

            await m_dbWrapper.ReadTables();

            int iFinding = 0;

            // Trainer has to be participant of training

            DateTime dateStart = new DateTime(2017, 1, 1);

            foreach (Training training in m_dbWrapper.AllTrainings())
            {
                int? trainer = training.Trainer;
                if ( ( training.Termin > dateStart ) && (trainer.HasValue && trainer > 0) )
                {
                    if (!m_dbWrapper.HatTeilgenommen(training.Trainer, training))
                    {
                        ofile.Write($"Finding { ++iFinding }: Trainer hat nicht an Training teilgenommen. ");
                        ofile.WriteMitglied(m_dbWrapper.MitgliedFromId(trainer.Value));
                        ofile.Write( $" Training Nr. {training.Id} " );
                        ofile.WriteTraining(training, m_dbWrapper.WeekDay(training.WochentagID));
                        ofile.WriteLine("");
                    }
                }
            }

            // Prüfungen:
            // Am Prüfungstag muss ein Training stattgefunden haben
            // Prüfling and Prüfer müssen Teilnehmer sein

            foreach (Pruefung pruefung in m_dbWrapper.Pruefungen())
            {
                bool found = false;
                foreach (Training training in m_dbWrapper.AllTrainings())
                {
                     if ( 
                          ( training.Termin == pruefung.Datum ) &&
                          ( m_dbWrapper.HatTeilgenommen(pruefung.Pruefling, training )) &&
                          ( m_dbWrapper.HatTeilgenommen(pruefung.Pruefer,   training ))
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
                    ofile.WritePruefung(pruefung, m_dbWrapper);
                    ofile.Write( "Prüfling: " );
                    ofile.WriteMitglied(m_dbWrapper.MitgliedFromId( pruefung.Pruefling ));
                    ofile.WriteLine();
                }
            }

            ofile.Close();
        }
    }
}
