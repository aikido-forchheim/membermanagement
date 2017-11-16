using System;
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
        List<Training>               m_trainings;
        List<Mitglied>               m_mitglieder;
        List<TrainerErnennung>       m_trainerErnennungen;
        List<Stundensatz>            m_stundensaetze;
        List<ZuschlagKindertraining> m_zuschlagKinderTraining;
        List<TrainerStufe>           m_trainerStufe;
        List<Wohnung>                m_wohnung;
        List<Wohnungsbezug>          m_wohnungsbezug;

        decimal[] m_decVerguetung;
        decimal[] m_decFahrtkosten;

        private void addTrainerVerguetung( Training training, int? trainerId, int trainerNr )
        {
            if ( trainerId.HasValue && (trainerId >= 0) )
            {
                var ernennungen  = m_trainerErnennungen.Where( t => (t.MitgliedID == trainerId) && ((t.Datum == null) || (t.Datum <= training.Termin)) );
                var trainerstufe = ernennungen.Any() ? ernennungen.Max(t => t.StufeID) : 1;
                var stufenName   = m_trainerStufe.Single(s => (s.Id == trainerstufe)).Bezeichnung;
                var stundensatz  = m_stundensaetze.Single(s => ( s.TrainerStufenID == trainerstufe) && (s.TrainerNummer == trainerNr) && (s.Dauer == training.DauerMinuten) );
                var trainer      = m_mitglieder.Single(m => m.Id == trainerId);

                decimal verguetung = stundensatz.Betrag;

                if ( trainerNr == 1 )
                    System.Console.Write($"{training.Termin:yyyy-MM-dd} {training.Zeit:hh}:{training.Zeit:mm}, {training.DauerMinuten,3} min, ");
                else
                    System.Console.Write($"                           ");

                System.Console.Write($"{trainerNr}. Trainer, {trainer.Nachname, -10} {trainer.Vorname, -9}({trainerId, 3}), Stufe {trainerstufe}, {verguetung, 5}" );

                if ( training.Kindertraining )
                {
                    var zuschlag = m_zuschlagKinderTraining.Single( s => (s.Trainernummer == trainerNr) && (s.Dauer == training.DauerMinuten) );
                    System.Console.Write( $" + {zuschlag.Betrag, 4}" );
                    verguetung += zuschlag.Betrag;
                }
                else
                    System.Console.Write( $"       ");

                System.Console.Write( $" = { verguetung, 5 }" );

                var wohnungsbezug = m_wohnungsbezug.Where(t => (t.MitgliedId == trainerId) && ((t.Datum == null) || (t.Datum <= training.Termin)));
                if ( wohnungsbezug.Any() )
                {
                    var letzterUmzug = wohnungsbezug.Max(t => t.Datum);
                    var wohnungId    = wohnungsbezug.Single(s => (s.Datum == letzterUmzug)).WohnungId;
                    var wohnung      = m_wohnung.Single(s => (s.Id == wohnungId));

                    if (wohnung.Fahrtstrecke.HasValue)
                    {
                        decimal FKZ = Decimal.Round( wohnung.Fahrtstrecke.Value * 0.17M, 2 );
                        System.Console.Write($", FKZ = { FKZ,5 }");
                        m_decFahrtkosten[trainerId.Value] += FKZ;
                    }
                }
                m_decVerguetung[trainerId.Value] += verguetung;
                System.Console.WriteLine();
            }
        }

        public async Task Main()
        {
            m_trainings              = await Program.Container.Resolve < IRepository < Training >               > ().GetAsync();
            m_mitglieder             = await Program.Container.Resolve < IRepository < Mitglied >               > ().GetAsync();
            m_trainerErnennungen     = await Program.Container.Resolve < IRepository < TrainerErnennung >       > ().GetAsync();
            m_stundensaetze          = await Program.Container.Resolve < IRepository < Stundensatz >            > ().GetAsync();
            m_zuschlagKinderTraining = await Program.Container.Resolve < IRepository < ZuschlagKindertraining > > ().GetAsync();
            m_trainerStufe           = await Program.Container.Resolve < IRepository < TrainerStufe>            > ().GetAsync();
            m_wohnung                = await Program.Container.Resolve < IRepository < Wohnung >                > ().GetAsync();
            m_wohnungsbezug          = await Program.Container.Resolve < IRepository < Wohnungsbezug >          > ().GetAsync();

            int iArraySize = m_mitglieder.Max(t => t.Id) + 1;

            m_decVerguetung  = new decimal[iArraySize];
            m_decFahrtkosten = new decimal[iArraySize];

            DateTime periodStart = new DateTime(2016, 1, 1);
            DateTime periodEnd   = new DateTime(2016, 12, 31);

            var trainingsInPeriod = m_trainings.Where(training => training.Termin > periodStart && training.Termin < periodEnd).ToList();

            foreach (var training in trainingsInPeriod)
            {
                addTrainerVerguetung(training, training.Trainer,    1);
                addTrainerVerguetung(training, training.Kotrainer1, 2);
                addTrainerVerguetung(training, training.Kotrainer2, 3);
            }

            System.Console.WriteLine();

            decimal summeUnterricht = 0;
            decimal summeFKZ        = 0;
            foreach ( var mitglied in m_mitglieder )
            {
                if ( mitglied.Id >= 0 )
                {
                    decimal betrag = m_decVerguetung[mitglied.Id];
                    decimal FKZ    = m_decFahrtkosten[mitglied.Id];

                    if (betrag > 0)
                    {
                        System.Console.WriteLine($"{ mitglied.Nachname, -10 } { mitglied.Vorname, -10 } ({ mitglied.Id, 3 }): Unterricht: { betrag, 8 }, FKZ: { FKZ, 8 } ");
                        summeUnterricht += betrag;
                        summeFKZ += FKZ;
                    }
                }
            }

            System.Console.WriteLine();
            System.Console.WriteLine($"Insgesamt Unterricht: { summeUnterricht,            8 }");
            System.Console.WriteLine($"Insgesamt FKZ       : { summeFKZ,                   8 }");
            System.Console.WriteLine($"Gesamtsumme         : { summeUnterricht + summeFKZ, 8 }");
        }
    }
}