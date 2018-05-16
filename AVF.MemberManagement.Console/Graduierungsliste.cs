using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

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
        internal async Task Main()
        {
            OutputTarget oTarget = new OutputTarget( "Graduierungsliste.txt" );

            List<ComparableMember> m_memberList = new List<ComparableMember>();

            DateTime dateZero = new DateTime(1, 1, 1);

            foreach (Mitglied mitglied in Globals.DatabaseWrapper.P_mitglieder)
            {
                if (Globals.DatabaseWrapper.IstNochMitglied(mitglied.Id))
                {
                    int      iStartGrad = (mitglied.BeitragsklasseID == 4) ? 1 : 7; // Erwachsene und Jugendliche beginnen mit dem 6. Kyu
                    DateTime iStartDate = (mitglied.Eintritt.HasValue) ? mitglied.Eintritt.Value : dateZero;
                    ComparableMember member = new ComparableMember(mitglied.Id, iStartGrad, iStartDate);

                    foreach (Pruefung pruefung in Globals.DatabaseWrapper.P_pruefung)
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
            DateTime datFirstReliableData = Globals.DatabaseWrapper.GetStartValidData();
            List<TrainingsTeilnahme> tn = Globals.DatabaseWrapper.P_trainingsTeilnahme;
            foreach (ComparableMember cmem in m_memberList)
            {
                Graduierung grad     = Globals.DatabaseWrapper.GraduierungFromId(cmem.GetGraduierung());
                Graduierung gradNext = Globals.DatabaseWrapper.GraduierungFromId(cmem.GetGraduierung() + 1);
                Mitglied    mitglied = Globals.DatabaseWrapper.MitgliedFromId(cmem.GetMemberId());
                DateTime    dateGrad = cmem.GetDatumGraduierung();
//                DateTime    dateNext = Graduation.MinDateGradNext(gradNext, dateGrad); 
                var         tnMember = Globals.DatabaseWrapper.Filter(tn, mitglied.Id);
                TimeRange   range    = new TimeRange(dateGrad, DateTime.Now);
                int         iCount   = Globals.DatabaseWrapper.Filter(tnMember, range).Count;

                string sGrad = "";
                if (grad.Id != iGradIdLast)
                {
                    sGrad = $"{grad.Bezeichnung} ";
                    if ( grad.Id > 1 )
                        sGrad += $"({grad.Japanisch}) ";
                    iGradIdLast = grad.Id;
                }
                oTarget.Write(sGrad.PadRight(20));
                oTarget.WriteMitglied( mitglied );
                oTarget.Write($"{ mitglied.Geburtsdatum:dd.MM.yyyy} ");
//                oTarget.Write($"{ mitglied.Geburtsort, -20} ");
                oTarget.Write($"{ Globals.DatabaseWrapper.BK_Text(mitglied), 3} ");
                oTarget.Write($"{ mitglied.Eintritt:dd.MM.yyyy} ");
                oTarget.Write($"{ dateGrad:dd.MM.yyyy} ");
//                oTarget.Write($"{ dateNext:dd.MM.yyyy} ");
                oTarget.Write((dateGrad < datFirstReliableData) ? "> " : "  ");
                oTarget.Write($" { iCount }");
                oTarget.WriteLine( );
            }

            oTarget.CloseAndReset2Console();
        }
    }
}
