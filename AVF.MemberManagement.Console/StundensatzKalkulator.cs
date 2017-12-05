using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.Console
{
    internal class StundensatzKalkulator
    {
        private List<Training>               m_trainings;
        private List<Mitglied>               m_mitglieder;
        private List<TrainerErnennung>       m_trainerErnennungen;
        private List<Stundensatz>            m_stundensaetze;
        private List<ZuschlagKindertraining> m_zuschlagKinderTraining;
        private List<TrainerStufe>           m_trainerStufe;
        private List<Wohnung>                m_wohnung;
        private List<Wohnungsbezug>          m_wohnungsbezug;
        private List<Wochentag>              m_wochentag;

        struct TrainerVerguetung
        {
            public decimal decVerguetung;
            public decimal decFahrtkosten;
        };

        private TrainerVerguetung[] m_aTrData;

        // table access functions

        private int TrainerLevel( int? trainerId, DateTime termin )  // calculates level (Vereinstrainer, Lehrer ...) of a trainer at a given date
        {
            var ernennungen = m_trainerErnennungen.Where(t => (t.MitgliedID == trainerId) && ((t.Datum == null) || (t.Datum <= termin)));
            return ernennungen.Any() ? ernennungen.Max(t => t.StufeID) : 1;
        }

        private string WeekDay(int id)
        {
            var wochentag = m_wochentag.Single(s => s.Id == id);
            return wochentag.Bezeichnung;
        }

        private decimal TrainingAward( int level, int duration, int trainerNr )
        {
            var stundensatz = m_stundensaetze.Single(s => (s.TrainerStufenID == level) && (s.TrainerNummer == trainerNr) && (s.Dauer == duration));
            return stundensatz.Betrag;
        }

        private decimal ExtraAmount( Training training, int trainerNr )
        {
            if (training.Kindertraining)
            {
                var zuschlag = m_zuschlagKinderTraining.Single(s => (s.Trainernummer == trainerNr) && (s.Dauer == training.DauerMinuten));
                return zuschlag.Betrag;
            }
            return 0;
        }

        private decimal TravelExpenses(int? idMitglied, DateTime termin)
        {
            var wohnungsbezug = m_wohnungsbezug.Where(t => (t.MitgliedId == idMitglied) && ((t.Datum == null) || (t.Datum <= termin)));
            if (wohnungsbezug.Any())
            {
                var letzterUmzug = wohnungsbezug.Max(t => t.Datum);
                var wohnungId    = wohnungsbezug.Single(s => (s.Datum == letzterUmzug)).WohnungId;
                var wohnung      = m_wohnung.Single(s => (s.Id == wohnungId));
                if (wohnung.Fahrtstrecke.HasValue)
                {
                    return Decimal.Round(wohnung.Fahrtstrecke.Value * 0.17M, 2);
                }
            }
            return 0;
        }

        // basic output functions

        private void WriteTraining(System.IO.StreamWriter ofile, Training training)
        {
            ofile.Write($"{training.Termin:yyyy-MM-dd} {WeekDay(training.WochentagID),-10} {training.Zeit:hh}:{training.Zeit:mm}, {training.DauerMinuten,3} min, ");
        }

        private void WriteMitglied(System.IO.StreamWriter ofile, Mitglied mitglied)
        {
            ofile.Write($"{ mitglied.Nachname,-10 } { mitglied.Vorname,-10 } ({ mitglied.Id,3 }) ");
        }

        private void WriteAmount(System.IO.StreamWriter ofile, string prefix, decimal amount)
        {
            string s = $"{prefix} { amount,7 } € ";

            if (amount > 0)
            {
                ofile.Write( s );
            }
            else
            {
                for (int i = 0; i < s.Length; ++i)
                    ofile.Write($" ");
            }
        }

        private void WriteSum(System.IO.StreamWriter ofile, decimal amount1,  decimal amount2 )
        {
            WriteAmount(ofile, " ", amount1);
            WriteAmount(ofile, "+", amount2);
            WriteAmount(ofile, "=", amount1 + amount2);
        }

        // helper functions

        private int? TrainerIdFromNr(Training training, int trainerNr )
        {
            switch ( trainerNr )
            {
                case 1: return training.Trainer;
                case 2: return training.Kotrainer1;
                case 3: return training.Kotrainer2;
                default:
                    // throw exception
                    return null;
            }
        }

        private decimal HandleTrainerFee( System.IO.StreamWriter ofile, Training training, int? trainerId, int trainerNr )
        {
            var trainerstufe = TrainerLevel( trainerId, training.Termin );

            ofile.Write( $"{trainerNr}. Trainer, " );
            ofile.Write( $" Stufe { trainerstufe }, " );

            if (training.VHS)
            {
                ofile.Write($"  -------------- VHS -------------- ");
                return 0;
            }
            else
            {
                decimal grundVerguetung        = TrainingAward(trainerstufe, training.DauerMinuten, trainerNr);
                decimal zuschlagKinderTraining = ExtraAmount(training, trainerNr);
                decimal gesamtVerguetung       = grundVerguetung + zuschlagKinderTraining;

                WriteSum(ofile, grundVerguetung, zuschlagKinderTraining);

                return gesamtVerguetung;
            }
        }

        private void AddTrainerVerguetung
        ( 
            System.IO.StreamWriter ofile, 
            Training               training, 
            int                    trainerNr,
            bool                   writeMitglied
        )
        {
            int? trainerId = TrainerIdFromNr(training, trainerNr);

            if ( ! trainerId.HasValue ) return;
            if (   trainerId < 0 )      return;

            WriteTraining( ofile, training );

            if ( writeMitglied )
                WriteMitglied( ofile, m_mitglieder.Single(s => s.Id == trainerId) );

            decimal trainerFee  = HandleTrainerFee( ofile, training, trainerId, trainerNr );    // Vergütung für Unterrichtstätigkeit
            decimal fahrtKosten = TravelExpenses( trainerId, training.Termin );                 // Fahrtkosten

            m_aTrData[trainerId.Value].decVerguetung  += trainerFee;
            m_aTrData[trainerId.Value].decFahrtkosten += fahrtKosten;

            WriteAmount( ofile, " ", fahrtKosten );

            WriteAmount( ofile, "  Summe ", trainerFee + fahrtKosten );
            ofile.WriteLine();
        }

        private void SettlePeriod
        ( 
            System.IO.StreamWriter ofile, 
            Mitglied               trainer,                       // null means: no specific trainer, do for all trainers
            List<Training>         trainings,
            bool                   writeMitglied
        )
        {
            foreach (Training training in trainings)
            {
                for (int trainerNr = 1; trainerNr <= 3; ++trainerNr)
                    if ((trainer == null) || (TrainerIdFromNr(training, trainerNr) == trainer.Id))
                        AddTrainerVerguetung(ofile, training, trainerNr, writeMitglied);
            }
        }

        private void CreateIndividualAccount( Mitglied trainer, List<Training> trainings )
        {
            string fileName = $"{ trainer.Nachname }_{ trainer.Vorname }.txt  ";

            System.IO.StreamWriter ofile = new System.IO.StreamWriter( fileName );

            WriteMitglied( ofile, trainer );
            ofile.WriteLine( );
            ofile.WriteLine( );

            SettlePeriod( ofile, trainer, trainings, false );

            ofile.WriteLine( );
            ofile.WriteLine( $" Unterricht  Fahrtkosten    Summe") ;
            ofile.WriteLine( ) ;

            WriteSum
            (
                ofile, 
                m_aTrData[trainer.Id].decVerguetung, 
                m_aTrData[trainer.Id].decFahrtkosten
            );

            ofile.WriteLine();
            ofile.Close();
        }

        private void WriteSummary(System.IO.StreamWriter ofile)
        {
            ofile.WriteLine( );
            ofile.Write    ( $"Nachname   Vorname     Nr.  " );
            ofile.WriteLine( $" Unterricht  Fahrtkosten    Summe" );
            ofile.WriteLine( );

            decimal summeUnterricht = 0;
            decimal summeFahrtkosten = 0;
            foreach (Mitglied mitglied in m_mitglieder)
            {
                if (mitglied.Id >= 0)
                {
                    decimal unterricht = m_aTrData[mitglied.Id].decVerguetung;
                    decimal fahrtKosten = m_aTrData[mitglied.Id].decFahrtkosten;

                    if ((unterricht > 0) || (fahrtKosten > 0))
                    {
                        WriteMitglied(ofile, mitglied);

                        WriteSum(ofile, unterricht, fahrtKosten);
                        ofile.WriteLine();

                        summeUnterricht += unterricht;
                        summeFahrtkosten += fahrtKosten;
                    }
                }
            }

            ofile.WriteLine();
            ofile.Write($"Gesamtbeträge               ");
            WriteSum(ofile, summeUnterricht, summeFahrtkosten);
            ofile.WriteLine();
        }

        private async Task ReadTables()
        {
            m_trainings = await Program.Container.Resolve<IRepository<Training>>().GetAsync();
            m_mitglieder = await Program.Container.Resolve<IRepository<Mitglied>>().GetAsync();
            m_trainerErnennungen = await Program.Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            m_stundensaetze = await Program.Container.Resolve<IRepository<Stundensatz>>().GetAsync();
            m_zuschlagKinderTraining = await Program.Container.Resolve<IRepository<ZuschlagKindertraining>>().GetAsync();
            m_trainerStufe = await Program.Container.Resolve<IRepository<TrainerStufe>>().GetAsync();
            m_wohnung = await Program.Container.Resolve<IRepository<Wohnung>>().GetAsync();
            m_wohnungsbezug = await Program.Container.Resolve<IRepository<Wohnungsbezug>>().GetAsync();
            m_wochentag = await Program.Container.Resolve<IRepository<Wochentag>>().GetAsync();
        }

        public async Task Main()
        {
            await ReadTables();

            int iArraySize = m_mitglieder.Max(t => t.Id) + 1;

            m_aTrData = new TrainerVerguetung[iArraySize];

            DateTime datStart = new DateTime(2017,  1,  1);
            DateTime datEnd   = new DateTime(2017, 12, 31);

            List<Training> trainingsInPeriod = m_trainings.Where(training => training.Termin > datStart && training.Termin < datEnd).ToList();

            {
                System.IO.StreamWriter ofile = new System.IO.StreamWriter(@"Trainerverguetung.txt");

                SettlePeriod( ofile, null, trainingsInPeriod, true );

                WriteSummary(ofile);

                ofile.Close();
            }

            foreach (Mitglied mitglied in m_mitglieder)
            {
                if ( ( mitglied.Id >= 0 ) && ( m_aTrData[mitglied.Id].decFahrtkosten > 0 ) )
                {
                    CreateIndividualAccount( mitglied, trainingsInPeriod );
                }
            }

        }
    }
}