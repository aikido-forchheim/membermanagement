using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class BestGraduation : IComparable<BestGraduation>
    {
        public int      m_memberId         { get; private set; }
        public int      m_graduierung      { get; private set; }
        public DateTime m_datumGraduierung { get; private set; }
        public DateTime m_datumMinNextGrad { get; private set; }
        public int      m_trainingsNeeded  { get; private set; }

        public BestGraduation(int memberId, int grad, DateTime date)
        {
            m_memberId = memberId;
            m_graduierung = grad;
            m_datumGraduierung = date;
            Graduierung gradNext = Globals.DatabaseWrapper.GraduierungFromId(m_graduierung + 1);
            m_datumMinNextGrad = m_datumGraduierung.AddYears(gradNext.WartezeitJahre).AddMonths(gradNext.WartezeitMonate);
            m_trainingsNeeded = ((m_datumMinNextGrad - m_datumGraduierung).Days / 7) * 2;  // 2 trainings per week needed
        }

        public void Complete( )
        {
            Graduierung gradNext = Globals.DatabaseWrapper.GraduierungFromId(m_graduierung + 1);
            m_datumMinNextGrad = m_datumGraduierung.AddYears(gradNext.WartezeitJahre).AddMonths(gradNext.WartezeitMonate);
            m_trainingsNeeded = ((m_datumMinNextGrad - m_datumGraduierung).Days / 7) * 2;  // 2 trainings per week needed
        }

        public int CompareTo(BestGraduation other)
        {
            if (m_graduierung > other.m_graduierung)
                return -1;
            else if (m_graduierung < other.m_graduierung)
                return 1;
            else if (m_datumGraduierung < other.m_datumGraduierung)
                return -1;
            else if (m_datumGraduierung > other.m_datumGraduierung)
                return 1;
            else if (m_memberId < other.m_memberId)
                return -1;
            else
                return 1;
        }

        public void ReplaceIfBetter(int grad, DateTime date)
        {
            if (grad > m_graduierung)
            {
                m_graduierung = grad;
                m_datumGraduierung = date;
            }
        }
    }

    public class BestGraduationList
    {
        public List<BestGraduation> m_listBestGraduation { get; private set; }

        public BestGraduationList()
        {
            m_listBestGraduation = new List<BestGraduation>();

            DateTime dateZero = new DateTime(1, 1, 1);

            foreach (Mitglied mitglied in Globals.DatabaseWrapper.CurrentMembers())
            {
                int      iStartGrad = (mitglied.BeitragsklasseID == 4) ? 1 : 7; // Erwachsene und Jugendliche beginnen mit dem 6. Kyu
                DateTime iStartDate = (mitglied.Eintritt.HasValue) ? mitglied.Eintritt.Value : dateZero;
                BestGraduation best = new BestGraduation(mitglied.Id, iStartGrad, iStartDate);

                foreach (Pruefung pruefung in Globals.DatabaseWrapper.m_pruefung)
                {
                    if (pruefung.Pruefling == mitglied.Id)
                    {
                        best.ReplaceIfBetter(pruefung.GraduierungID, pruefung.Datum);
                    }
                }

                best.Complete();
                m_listBestGraduation.Add(best);
            }

            m_listBestGraduation.Sort();
        }
    }
}
