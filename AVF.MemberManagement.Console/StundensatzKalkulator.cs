using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

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

        public void WriteSum( OutputTarget o )
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
            TrainerAward   ta,
            OutputTarget   oTarget, 
            Mitglied       trainer,                       // null means: no specific trainer, do for all trainers
            List<Training> trainings,
            bool           print
        )
        {
            if (oTarget == null)
            {
                throw new System.ArgumentNullException(nameof(oTarget));
            }

            foreach (Training training in trainings)
            {
                for (int trainerNr = 1; trainerNr <= 3; ++trainerNr)
                {
                    if ((trainer == null) || (TrainerIdFromNr(training, trainerNr) == trainer.Id))
                    {
                        int? trainerId = TrainerIdFromNr(training, trainerNr);

                        if ( ta.IstNochMitglied(trainerId) )
                        {
                            int trainerstufe = ta.TrainerLevel(trainerId, training.Termin);

                            TrainerVerguetung tv = new TrainerVerguetung
                            (
                                ta.TrainingAward(trainerstufe, training, trainerNr),
                                ta.ExtraAmount(training, trainerNr),
                                ta.CalcTravelExpenses(trainerId, training.Termin)
                            );

                            m_aTrData[trainerId.Value].Add(tv);

                            if ( print )
                            {
                                oTarget.WriteTraining(training, training.WochentagID);
                                oTarget.Write($"{trainerNr} ");
                                oTarget.Write($"{trainerstufe} ");

                                if (training.VHS)
                                {
                                    oTarget.Write($"  ------- VHS ------- ");
                                }
                                else
                                {
                                    oTarget.WriteAmount( tv.GrundVerguetung());
                                    oTarget.WriteAmount( tv.ZuschlagKindertraining());
                                }

                                oTarget.WriteAmount( tv.Fahrtkosten());
                                oTarget.WriteAmount( tv.Summe());
                                oTarget.WriteLine();
                            }
                        }
                    }
                }
            }
        }

        private TrainerVerguetung[] InitTrData( )
        {
            TrainerVerguetung[] aTrData = new TrainerVerguetung[Globals.DatabaseWrapper.MaxMitgliedsNr() + 1];

            for (int i = 0; i < aTrData.Length; i++)
            {
                aTrData[i] = new TrainerVerguetung();
            }

            return aTrData;
        }

        internal async Task Main( int iJahr )
        {
            m_aTrData = InitTrData();

            var trainingsInPeriod = Globals.DatabaseWrapper.TrainingsInPeriod( -1, iJahr );
            var trainerAward = new TrainerAward();

            // Create summary account

            {
                OutputTarget oTarget = new OutputTarget( @"Trainerverguetung.txt" );

                SettlePeriod(trainerAward, oTarget, null, trainingsInPeriod, false );

                oTarget.WriteLine();
                oTarget.WriteLine($"Nachname   Vorname     Nr.   Unterricht  Fahrtkosten    Summe");
                oTarget.WriteLine();

                TrainerVerguetung tvSum = new TrainerVerguetung(0, 0, 0);

                foreach (Mitglied mitglied in Globals.DatabaseWrapper.P_mitglieder)
                {
                    TrainerVerguetung tv = m_aTrData[mitglied.Id];

                    if (!tv.IsEmpty())
                    {
                        oTarget.WriteMitglied(mitglied);

                        tv.WriteSum(oTarget);
                        oTarget.WriteLine();

                        tvSum.Add(tv);
                    }
                }

                oTarget.WriteLine();
                oTarget.Write($"Gesamtbeträge                   ");
                tvSum.WriteSum(oTarget);
                oTarget.WriteLine();

                oTarget.CloseAndReset2Console();
            }

         // Create individual accounts

            foreach ( Mitglied mitglied in Globals.DatabaseWrapper.P_mitglieder )
            {
                if ( ! m_aTrData[mitglied.Id].IsEmpty( ) )    // filled by summary account calculation if trainer
                {
                    OutputTarget oTarget = new OutputTarget( $"{ mitglied.Nachname }_{ mitglied.Vorname }.txt  " );

                    oTarget.WriteLine( $"{ mitglied.Nachname,-10 } { mitglied.Vorname,-10 } MitgliedsNr. ({ mitglied.Id,3 }) ") ;
                    oTarget.WriteLine();

                    m_aTrData[mitglied.Id].SetEmpty();     // reset individual account to avoid double counting

                    oTarget.WriteLine("Tag des Trainings     Zeit  Min T T  Grundver-    Kinder-     Fahrt-    Summe");
                    oTarget.WriteLine("                                N S   guetung     zuschlag    kosten         ");
                    oTarget.WriteLine();

                    SettlePeriod(trainerAward, oTarget, mitglied, trainingsInPeriod, true );

                    oTarget.WriteLine();
                    oTarget.Write      ("Summe                               ");
                    oTarget.WriteAmount( m_aTrData[mitglied.Id].GrundVerguetung());
                    oTarget.WriteAmount( m_aTrData[mitglied.Id].ZuschlagKindertraining());
                    oTarget.WriteAmount( m_aTrData[mitglied.Id].Fahrtkosten());
                    oTarget.WriteAmount( m_aTrData[mitglied.Id].Summe());
                    oTarget.WriteLine();
                    oTarget.WriteLine();

                    oTarget.WriteLine("TN: Trainernummer");
                    oTarget.WriteLine("1 - Hauptrainer");
                    oTarget.WriteLine("2 - Erster Kotrainer");
                    oTarget.WriteLine("3 - Zweiter Kotrainer");
                    oTarget.WriteLine();
                    oTarget.WriteLine("TS: Trainerstufe");
                    for (int i = 1; i <= Globals.DatabaseWrapper.MaxTrainerstufe; i++)
                        oTarget.WriteLine( $"{ i } - { Globals.DatabaseWrapper.Trainerstufe( i )}" );

                    oTarget.CloseAndReset2Console();
                }
            }
        }
    }
}