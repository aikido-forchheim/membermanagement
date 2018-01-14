using System;
using System.Linq;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Utilities;

namespace AVF.MemberManagement.Console
{
    internal class BusinessLogic
    {
        private DatabaseWrapper m_dbWrapper;

        public class KursTeilnahme
        {
            public int m_idKurs;        // identifier of Kurs
            public int m_nrTeilnahmen;  // number of partications

            public KursTeilnahme(int idKurs)
            {
                m_idKurs = idKurs;
                m_nrTeilnahmen = 0;
            }

            public void Increase( )
            {
                ++ m_nrTeilnahmen;
            }

            public int IdKurs( )
            {
                return m_idKurs;
            }

            public int NrTeilnahmen( )
            {
                return m_nrTeilnahmen;
            }
        }

        public class MitgliedKurse
        {
            private int                 m_idMember;
            private List<KursTeilnahme> m_listParticipationsPerCourse;

            public MitgliedKurse( int idMember )
            {
                m_idMember = idMember;
                m_listParticipationsPerCourse = new List<KursTeilnahme>();
            }

            public void Add( KursTeilnahme kt )
            {
                m_listParticipationsPerCourse.Add(kt);
            }

            public int IdMember( )
            {
                return m_idMember;
            }

            public List<KursTeilnahme> List( )
            {
                return m_listParticipationsPerCourse;
            }
        }

        public BusinessLogic( DatabaseWrapper db )
        {
            m_dbWrapper = db;
        }

        public List<MitgliedKurse> TeilnahmeKurs( DateTime datStart, DateTime datEnd )
        {
            var result = new List<MitgliedKurse>();

            var trainingsInTimeRange = m_dbWrapper.AllTrainings().Where(training => training.Termin > datStart && training.Termin < datEnd).ToList();
            trainingsInTimeRange = trainingsInTimeRange.OrderBy(x => x.Termin).ToList();

            foreach (var mitglied in m_dbWrapper.Mitglieder())
            {
                if (m_dbWrapper.HatTeilgenommen(mitglied.Id, trainingsInTimeRange))
                {
                    MitgliedKurse mk = new MitgliedKurse(mitglied.Id);
                    result.Add( mk );
                
                    foreach (var kurs in m_dbWrapper.Kurse())
                    {
                        var relevantTrainings = trainingsInTimeRange.Where(training => training.KursID == kurs.Id).ToList();

                        if (m_dbWrapper.HatTeilgenommen(mitglied.Id, relevantTrainings))
                        {
                            KursTeilnahme kt = new KursTeilnahme( kurs.Id );

                            foreach (var training in relevantTrainings)
                                if (m_dbWrapper.HatTeilgenommen(mitglied.Id, training))
                                    kt.Increase();

                            mk.Add(kt);
                        }
                    }
                }
            }

            return result;
        }
    }
}
