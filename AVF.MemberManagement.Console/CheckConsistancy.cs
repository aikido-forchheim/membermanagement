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

            foreach (Training training in m_dbWrapper.AllTrainings())
            {
                int ? trainer = training.Trainer;
                if (trainer.HasValue && trainer > 0)
                {
                    if (!m_dbWrapper.HatTeilgenommen(training.Trainer, training))
                        ofile.WriteLine( $"Finding { ++iFinding }: Trainer not participant of training" );
                    ofile.WriteMitglied(m_dbWrapper.MitgliedFromId(trainer.Value));
                    ofile.WriteTraining(training, m_dbWrapper.WeekDay(training.WochentagID));
                }
            }

            ofile.Close();
        }
    }
}
