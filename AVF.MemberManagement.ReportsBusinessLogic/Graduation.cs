using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class BestGraduation : IComparable<BestGraduation>
    {
        public int      m_memberId          { get; private set; }
        public int      m_graduierung       { get; private set; }
        public DateTime m_dateStart         { get; private set; }
        public DateTime m_datumGraduierung  { get; private set; }
        public DateTime m_datumMinNextGrad  { get; private set; }
        public int      m_trainingsNeeded   { get; private set; }
        public bool     m_fAllTrainingsInDb { get; private set; }
        public int      m_TrainngsDone      { get; private set; }
        public int      m_yearsOfMembership { get; private set; }

        private DateTime dateZero = new DateTime(1, 1, 1);

        public BestGraduation(Mitglied member)
        {
            m_graduierung      = (member.BeitragsklasseID == 4) ? 1 : 7; // Erwachsene und Jugendliche beginnen mit dem 6. Kyu
            m_datumGraduierung = (member.Eintritt.HasValue) ? member.Eintritt.Value : dateZero;
        }

        public void Complete(Mitglied member)
        {
            DateTime    datValidData = Globals.DatabaseWrapper.GetStartValidData();
            Graduierung gradNext     = Globals.DatabaseWrapper.GraduierungFromId(m_graduierung + 1);

            m_memberId          = member.Id;
            m_datumMinNextGrad  = m_datumGraduierung.AddYears(gradNext.WartezeitJahre).AddMonths(gradNext.WartezeitMonate);
            m_trainingsNeeded   = ((m_datumMinNextGrad - m_datumGraduierung).Days / 7) * 2;  // 2 trainings per week needed
            m_fAllTrainingsInDb = (datValidData <= m_datumGraduierung);
            m_dateStart         = m_fAllTrainingsInDb ? m_datumGraduierung : datValidData;
            m_TrainngsDone      = Globals.DatabaseWrapper.NrOfTrainingsSince(m_memberId, m_dateStart);
            m_yearsOfMembership = DateTime.Now.Year - member.Eintritt.Value.Year;
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
                m_graduierung      = grad;
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

            foreach (Mitglied member in Globals.DatabaseWrapper.CurrentMembers())
            {
                BestGraduation best = new BestGraduation(member);

                foreach (Pruefung pruefung in Globals.DatabaseWrapper.m_pruefung)
                {
                    if (pruefung.Pruefling == member.Id)
                    {
                        best.ReplaceIfBetter(pruefung.GraduierungID, pruefung.Datum);
                    }
                }

                best.Complete(member);
                m_listBestGraduation.Add(best);
            }

            m_listBestGraduation.Sort();
        }
    }
}
