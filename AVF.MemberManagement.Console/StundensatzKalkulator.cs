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
        List<Training>               m_trainings;
        List<Mitglied>               m_mitglieder;
        List<TrainerErnennung>       m_trainerErnennungen;
        List<Stundensatz>            m_stundensaetze;
        List<ZuschlagKindertraining> m_zuschlagKinderTraining;
        List<TrainerStufe>           m_trainerStufe;
        List<Wohnung>                m_wohnung;
        List<Wohnungsbezug>          m_wohnungsbezug;
        List<Wochentag>              m_wochentag;

        struct TrainerVerguetung
        {
            public decimal decVerguetung;
            public decimal decFahrtkosten;
            public int UL_counter;
            public int UM_counter;
            public int UK_counter;
            public int UKZ_counter;
        };

        TrainerVerguetung[] m_aTrData;

        System.IO.StreamWriter m_ofile;

        private void addTrainerVerguetung( Training training, int? trainerId, int trainerNr )
        {
            if ( trainerId.HasValue && (trainerId >= 0) )
            {
                var ernennungen  = m_trainerErnennungen.Where( t => (t.MitgliedID == trainerId) && ((t.Datum == null) || (t.Datum <= training.Termin)) );
                var trainerstufe = ernennungen.Any() ? ernennungen.Max(t => t.StufeID) : 1;
                var stufenName   = m_trainerStufe.Single(s => (s.Id == trainerstufe)).Bezeichnung;
                var stundensatz  = m_stundensaetze.Single(s => ( s.TrainerStufenID == trainerstufe) && (s.TrainerNummer == trainerNr) && (s.Dauer == training.DauerMinuten) );
                var trainer      = m_mitglieder.Single(s => s.Id == trainerId);
                var wochentag    = m_wochentag.Single(s => s.Id == training.WochentagID);

                decimal verguetung = stundensatz.Betrag;

 //               if ( trainerNr == 1 )
                    m_ofile.Write($"{training.Termin:yyyy-MM-dd} {wochentag.Bezeichnung, -10} {training.Zeit:hh}:{training.Zeit:mm}, {training.DauerMinuten,3} min, ");
 //               else
 //                   m_ofile.Write($"                                      ");

                m_ofile.Write($"{trainerNr}. Trainer, {trainer.Nachname,-10} {trainer.Vorname,-9}({trainerId,3}), Stufe {trainerstufe}, ");
                if (training.DauerMinuten == 120)
                {
                    m_ofile.Write($"UL  ");
                    ++ m_aTrData[trainerId.Value].UL_counter;
                }
                else if (training.DauerMinuten == 105)
                {
                    m_ofile.Write($"UM  ");
                    ++m_aTrData[trainerId.Value].UM_counter;
                }
                else if ( trainerNr == 1 )
                {
                    m_ofile.Write($"UK  ");
                    ++m_aTrData[trainerId.Value].UK_counter;
                }
                else
                {
                    m_ofile.Write($"UKZ ");
                    ++m_aTrData[trainerId.Value].UKZ_counter;
                }

                m_ofile.Write($"{verguetung, 5}" );

                if ( training.Kindertraining )
                {
                    var zuschlag = m_zuschlagKinderTraining.Single( s => (s.Trainernummer == trainerNr) && (s.Dauer == training.DauerMinuten) );
                    m_ofile.Write( $" + {zuschlag.Betrag, 4}" );
                    verguetung += zuschlag.Betrag;
                }
                else
                    m_ofile.Write( $"       ");

                m_ofile.Write( $" = { verguetung, 5 }" );

                var wohnungsbezug = m_wohnungsbezug.Where(t => (t.MitgliedId == trainerId) && ((t.Datum == null) || (t.Datum <= training.Termin)));
                if ( wohnungsbezug.Any() )
                {
                    var letzterUmzug = wohnungsbezug.Max(t => t.Datum);
                    var wohnungId    = wohnungsbezug.Single(s => (s.Datum == letzterUmzug)).WohnungId;
                    var wohnung      = m_wohnung.Single(s => (s.Id == wohnungId));

                    if (wohnung.Fahrtstrecke.HasValue)
                    {
                        decimal FKZ = Decimal.Round( wohnung.Fahrtstrecke.Value * 0.17M, 2 );
                        m_ofile.Write($", FKZ = { FKZ,5 }");
                        m_aTrData[trainerId.Value].decFahrtkosten += FKZ;
                    }
                }
                m_aTrData[trainerId.Value].decVerguetung += verguetung;
                m_ofile.WriteLine();
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
            m_wochentag              = await Program.Container.Resolve < IRepository < Wochentag>               > ().GetAsync();

            int iArraySize = m_mitglieder.Max(t => t.Id) + 1;

            m_aTrData = new TrainerVerguetung[iArraySize];

            m_ofile = new System.IO.StreamWriter( @"Trainerverguetung.txt" );
            
            DateTime periodStart = new DateTime(2016, 1, 1);
            DateTime periodEnd   = new DateTime(2016, 12, 31);

            var trainingsInPeriod = m_trainings.Where(training => training.Termin > periodStart && training.Termin < periodEnd).ToList();

            foreach (var training in trainingsInPeriod)
            {
                addTrainerVerguetung(training, training.Trainer,    1);
                addTrainerVerguetung(training, training.Kotrainer1, 2);
                addTrainerVerguetung(training, training.Kotrainer2, 3);
            }

            m_ofile.WriteLine();

            foreach (var mitglied in m_mitglieder)
            {
                if (mitglied.Id >= 0)
                {
                    if (m_aTrData[mitglied.Id].decVerguetung > 0)
                    {
                        m_ofile.WriteLine($"{ mitglied.Nachname, -10 } { mitglied.Vorname, -10 } ({ mitglied.Id, 3 })");
                        if (m_aTrData[mitglied.Id].UL_counter > 0)
                        {
                            m_ofile.WriteLine($"{ m_aTrData[mitglied.Id].UL_counter } UL  ");
                        }
                        if (m_aTrData[mitglied.Id].UM_counter > 0)
                        {
                            m_ofile.WriteLine($"{ m_aTrData[mitglied.Id].UM_counter } UM  ");
                        }
                        if (m_aTrData[mitglied.Id].UK_counter > 0)
                        {
                            m_ofile.WriteLine($"{ m_aTrData[mitglied.Id].UK_counter } UK  ");
                        }
                        if (m_aTrData[mitglied.Id].UKZ_counter > 0)
                        {
                            m_ofile.WriteLine($"{ m_aTrData[mitglied.Id].UKZ_counter } UKZ ");
                        }
                    }
                }
            }

            m_ofile.WriteLine();


            decimal summeUnterricht = 0;
            decimal summeFKZ        = 0;
            foreach ( var mitglied in m_mitglieder )
            {
                if ( mitglied.Id >= 0 )
                {
                    decimal betrag = m_aTrData[mitglied.Id].decVerguetung;
                    decimal FKZ    = m_aTrData[mitglied.Id].decFahrtkosten;

                    if (betrag > 0)
                    {
                        m_ofile.WriteLine($"{ mitglied.Nachname, -10 } { mitglied.Vorname, -10 } ({ mitglied.Id, 3 }): Unterricht: { betrag, 8 }, FKZ: { FKZ, 8 } ");
                        summeUnterricht += betrag;
                        summeFKZ += FKZ;
                    }
                }
            }

            m_ofile.WriteLine();
            m_ofile.WriteLine($"Insgesamt Unterricht: { summeUnterricht,            8 }");
            m_ofile.WriteLine($"Insgesamt FKZ       : { summeFKZ,                   8 }");
            m_ofile.WriteLine($"Gesamtsumme         : { summeUnterricht + summeFKZ, 8 }");

            m_ofile.Close();
        }
    }
}