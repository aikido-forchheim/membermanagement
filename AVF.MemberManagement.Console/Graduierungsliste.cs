using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Console
{
    public class ComparableMember : IComparable<ComparableMember>
    {
        private int      m_memberId;
        private int      m_graduierung;
        private DateTime m_datumGraduierung;

        public ComparableMember(int memberId, int grad, DateTime date) 
        {
            m_memberId = memberId;
            m_graduierung = grad;
            m_datumGraduierung = date;
        }

        public int GetMemberId()
        {
            return m_memberId;
        }

        public int GetGraduierung()
        {
            return m_graduierung;
        }

        public DateTime GetDatumGraduierung()
        {
            return m_datumGraduierung;
        }

        public int CompareTo(ComparableMember other)
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

        public void ReplaceIfBetter( int grad, DateTime date )
        {
            if ( grad > m_graduierung )
            {
                m_graduierung = grad;
                m_datumGraduierung = date;
            }
        }
    }

    class Graduierungsliste
    {
        private DatabaseWrapper m_dbWrapper;

        public async Task Main()
        {
            OutputFile ofile = new OutputFile("Graduierungsliste.txt");

            m_dbWrapper = new DatabaseWrapper();

            await m_dbWrapper.ReadTables();

            List<ComparableMember> m_memberList = new List<ComparableMember>();

            DateTime dateZero = new DateTime(1, 1, 1);

            foreach (Mitglied mitglied in m_dbWrapper.Mitglieder())
            {
                if (m_dbWrapper.IstNochMitglied(mitglied.Id))
                {
                    int      iStartGrad = (mitglied.BeitragsklasseID == 4) ? 1 : 7; // Erwachsene und Jugendliche beginnen mit dem 6. Kyu
                    DateTime iStartDate = (mitglied.Eintritt.HasValue) ? mitglied.Eintritt.Value : dateZero;
                    ComparableMember member = new ComparableMember(mitglied.Id, iStartGrad, iStartDate);

                    foreach (Pruefung pruefung in m_dbWrapper.Pruefungen())
                    {
                        if (pruefung.Pruefling == mitglied.Id)
                        {
                            member.ReplaceIfBetter(pruefung.GraduierungID, pruefung.Datum);
                        }
                    }

                    m_memberList.Add( member );
                }
            }

            m_memberList.Sort();

            int iGradIdLast = 0;
            foreach(ComparableMember cmem in m_memberList)
            {
                Graduierung grad     = m_dbWrapper.GraduierungFromId(cmem.GetGraduierung());
                Mitglied    mitglied = m_dbWrapper.MitgliedFromId(cmem.GetMemberId());
                DateTime    dateGrad = cmem.GetDatumGraduierung();
                DateTime    dateNext = dateGrad.AddYears(grad.WartezeitJahre).AddMonths(grad.WartezeitMonate);
                string      sGrad    = "";
                if (grad.Id != iGradIdLast)
                {
                    sGrad = $"{grad.Bezeichnung} ";
                    if ( grad.Id > 1 )
                        sGrad += $"({grad.Japanisch}) ";
                    iGradIdLast = grad.Id;
                }
                ofile.Write(sGrad.PadRight(20));
                ofile.WriteMitglied( mitglied );
                ofile.Write($"{ mitglied.Geburtsdatum:yyyy-MM-dd} ");
                ofile.Write($"{ mitglied.Geburtsort, -20} ");
                ofile.Write($"{ m_dbWrapper.Beitragsklasse(mitglied.BeitragsklasseID), 2} ");
                ofile.Write($"{ mitglied.Eintritt:yyyy-MM-dd} ");
                ofile.Write($"{ dateGrad:yyyy-MM-dd} ");
                ofile.Write($"{ dateNext:yyyy-MM-dd} ");
                ofile.WriteLine( );
            }

            ofile.Close();
        }
    }
}
