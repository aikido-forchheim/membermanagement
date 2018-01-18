using System;
//using System.IO;
//using System.Linq;
//using System.Diagnostics;
using System.Collections.Generic;
//using System.Windows.Forms.VisualStyles;
//using AVF.MemberManagement.StandardLibrary.Interfaces;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
//using Microsoft.Practices.ObjectBuilder2;
//using Microsoft.Practices.Unity;
using AVF.MemberManagement.BusinessLogic;

namespace AVF.MemberManagement.Console
{
    internal class TrainerVerguetung
    {
        public TrainerVerguetung()
        {
            SetEmpty();
        }

        public TrainerVerguetung(decimal g, decimal z, decimal f)
        {
            decGrundVerguetung = g;
            decZuschlagKindertraining = z;
            decFahrtkosten = f;
        }

        public void SetEmpty()
        {
            decGrundVerguetung = 0;
            decZuschlagKindertraining = 0;
            decFahrtkosten = 0;
        }

        public static TrainerVerguetung operator + ( TrainerVerguetung tv1, TrainerVerguetung tv2 )
        {
            return new TrainerVerguetung
            (
                tv1.decGrundVerguetung        + tv2.decGrundVerguetung,
                tv1.decZuschlagKindertraining + tv2.decZuschlagKindertraining,
                tv1.decFahrtkosten            + tv2.decFahrtkosten
            );
        }

        public void Add(TrainerVerguetung tv)
        {
            decGrundVerguetung        += tv.decGrundVerguetung;
            decZuschlagKindertraining += tv.decZuschlagKindertraining;
            decFahrtkosten            += tv.decFahrtkosten;
        }

        public bool IsEmpty( )
        {
            return (decGrundVerguetung == 0) && (decZuschlagKindertraining == 0) && (decFahrtkosten == 0);
        }

        public void WriteSum( OutputFile o )
        {
            o.WriteAmount( decGrundVerguetung + decZuschlagKindertraining );
            o.Write("+");
            o.WriteAmount( decFahrtkosten);
            o.Write("=");
            o.WriteAmount( decGrundVerguetung + decZuschlagKindertraining + decFahrtkosten );
        }

        public decimal Summe()
        {
            return decGrundVerguetung + decZuschlagKindertraining + decFahrtkosten;
        }

        public decimal Fahrtkosten()
        {
            return decFahrtkosten;
        }

        public decimal GrundVerguetung()
        {
            return decGrundVerguetung;
        }

        public decimal ZuschlagKindertraining()
        {
            return decZuschlagKindertraining;
        }

        private decimal decGrundVerguetung;
        private decimal decZuschlagKindertraining;
        private decimal decFahrtkosten;
    };

    internal class StundensatzKalkulator
    {
        private TrainerVerguetung[] m_aTrData;

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

        private void SettlePeriod
        (
            DatabaseWrapper db,
            OutputFile      ofile, 
            Mitglied        trainer,                       // null means: no specific trainer, do for all trainers
            List<Training>  trainings,
            bool            print
        )
        {
            foreach (Training training in trainings)
            {
                for (int trainerNr = 1; trainerNr <= 3; ++trainerNr)
                {
                    if ((trainer == null) || (TrainerIdFromNr(training, trainerNr) == trainer.Id))
                    {
                        int? trainerId = TrainerIdFromNr(training, trainerNr);

                        if ( db.IstNochMitglied(trainerId) )
                        {
                            int trainerstufe = db.TrainerLevel(trainerId, training.Termin);

                            TrainerVerguetung tv = new TrainerVerguetung
                            (
                                db.TrainingAward(trainerstufe, training, trainerNr),
                                db.ExtraAmount(training, trainerNr),
                                db.CalcTravelExpenses(trainerId, training.Termin)
                            );

                            m_aTrData[trainerId.Value].Add(tv);

                            if ( print )
                            {
                                ofile.WriteTraining(training, db.WeekDay(training.WochentagID));
                                ofile.Write($"{trainerNr} ");
                                ofile.Write($"{trainerstufe} ");

                                if (training.VHS)
                                {
                                    ofile.Write($"  ------- VHS ------- ");
                                }
                                else
                                {
                                    ofile.WriteAmount( tv.GrundVerguetung());
                                    ofile.WriteAmount( tv.ZuschlagKindertraining());
                                }

                                ofile.WriteAmount( tv.Fahrtkosten());
                                ofile.WriteAmount( tv.Summe());
                                ofile.WriteLine();
                            }
                        }
                    }
                }
            }
        }

        private TrainerVerguetung[] InitTrData( DatabaseWrapper db )
        {
            TrainerVerguetung[] aTrData = new TrainerVerguetung[db.MaxMitgliedsNr() + 1];

            for (int i = 0; i < aTrData.Length; i++)
            {
                aTrData[i] = new TrainerVerguetung();
            }

            return aTrData;
        }

        internal async Task Main( DatabaseWrapper db, int iJahr )
        {
            m_aTrData = InitTrData( db );

            var trainingsInPeriod = db.TrainingsInPeriod( -1, iJahr );

            // Create summary account

            {
                OutputFile ofile = new OutputFile( @"Trainerverguetung.txt", db );

                SettlePeriod( db, ofile, null, trainingsInPeriod, false );

                ofile.WriteLine();
                ofile.WriteLine($"Nachname   Vorname     Nr.   Unterricht  Fahrtkosten    Summe");
                ofile.WriteLine();

                TrainerVerguetung tvSum = new TrainerVerguetung(0, 0, 0);

                foreach (Mitglied mitglied in db.Mitglieder())
                {
                    TrainerVerguetung tv = m_aTrData[mitglied.Id];

                    if (!tv.IsEmpty())
                    {
                        ofile.WriteMitglied(mitglied);

                        tv.WriteSum(ofile);
                        ofile.WriteLine();

                        tvSum.Add(tv);
                    }
                }

                ofile.WriteLine();
                ofile.Write($"Gesamtbeträge                   ");
                tvSum.WriteSum(ofile);
                ofile.WriteLine();

                ofile.Close();
            }

         // Create individual accounts

            foreach ( Mitglied mitglied in db.Mitglieder( ) )
            {
                if ( ! m_aTrData[mitglied.Id].IsEmpty( ) )    // filled by summary account calculation if trainer
                {
                    OutputFile ofile = new OutputFile( $"{ mitglied.Nachname }_{ mitglied.Vorname }.txt  ", db );

                    ofile.WriteLine( $"{ mitglied.Nachname,-10 } { mitglied.Vorname,-10 } MitgliedsNr. ({ mitglied.Id,3 }) ") ;
                    ofile.WriteLine();

                    m_aTrData[mitglied.Id].SetEmpty();     // reset individual account to avoid double counting

                    ofile.WriteLine("Tag des Trainings     Zeit  Min T T  Grundver-    Kinder-     Fahrt-    Summe");
                    ofile.WriteLine("                                N S   guetung     zuschlag    kosten         ");
                    ofile.WriteLine();

                    SettlePeriod( db, ofile, mitglied, trainingsInPeriod, true );

                    ofile.WriteLine();
                    ofile.Write      ("Summe                               ");
                    ofile.WriteAmount( m_aTrData[mitglied.Id].GrundVerguetung());
                    ofile.WriteAmount( m_aTrData[mitglied.Id].ZuschlagKindertraining());
                    ofile.WriteAmount( m_aTrData[mitglied.Id].Fahrtkosten());
                    ofile.WriteAmount( m_aTrData[mitglied.Id].Summe());
                    ofile.WriteLine();
                    ofile.WriteLine();

                    ofile.WriteLine("TN: Trainernummer");
                    ofile.WriteLine("1 - Hauptrainer");
                    ofile.WriteLine("2 - Erster Kotrainer");
                    ofile.WriteLine("3 - Zweiter Kotrainer");
                    ofile.WriteLine();
                    ofile.WriteLine("TS: Trainerstufe");
                    for (int i = 1; i <= db.MaxTrainerstufe(); i++)
                        ofile.WriteLine( $"{ i } - { db.Trainerstufe( i )}" );

                    ofile.Close();
                }
            }
        }
    }
}