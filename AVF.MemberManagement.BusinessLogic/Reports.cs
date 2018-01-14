using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;


namespace AVF.MemberManagement.BusinessLogic
{
    public class Reports
    {
        private DatabaseWrapper m_dbWrapper;

        public struct KursTeilnahme
        {
            int kursId;       // identifier of Kurs
            int nTeilnahmen;  // number of partications
        }

        class MitgliedVsKurse
        {
            int memberId;
            List<KursTeilnahme> lParticipationsPerCourse;
        }

        List<MitgliedVsKurse> TeilnahmeKurs( DateTime datStart, DateTime datEnd )
        {
            return null;
        }
    }
}
