using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class BestGraduation : IComparable<BestGraduation>
    {
        public int      P_memberId          { get; private set; }
        public int      P_graduierung       { get; private set; }
        public DateTime P_dateStart         { get; private set; }
        public DateTime P_datumGraduierung  { get; private set; }
        public DateTime P_datumMinNextGrad  { get; private set; }
        public int      P_trainingsNeeded   { get; private set; }
        public bool     P_fAllTrainingsInDb { get; private set; }
        public int      P_TrainngsDone      { get; private set; }
        public int      P_yearsOfMembership { get; private set; }

        private DateTime dateZero = new DateTime(1, 1, 1);

        public BestGraduation(Mitglied member)
        {
            P_graduierung      = (member.BeitragsklasseID == 4) ? 1 : 7; // Erwachsene und Jugendliche beginnen mit dem 6. Kyu
            P_datumGraduierung = (member.Eintritt.HasValue) ? member.Eintritt.Value : dateZero;
        }

        public void Complete(Mitglied member)
        {
            DateTime    datValidData = Globals.DatabaseWrapper.GetStartValidData();
            Graduierung gradNext     = Globals.DatabaseWrapper.GraduierungFromId(P_graduierung + 1);

            //  datValidData = new DateTime(2016, 1, 1);  // TODO: remove this line

            P_memberId          = member.Id;
            P_datumMinNextGrad  = P_datumGraduierung.AddYears(gradNext.WartezeitJahre).AddMonths(gradNext.WartezeitMonate);
            P_trainingsNeeded   = gradNext.WartezeitJahre * 100 + (gradNext.WartezeitMonate * 100) / 12;   // ((P_datumMinNextGrad - P_datumGraduierung).Days / 7) * 2;  // 2 trainings per week needed
            P_fAllTrainingsInDb = (datValidData <= P_datumGraduierung);
            P_dateStart         = P_fAllTrainingsInDb ? P_datumGraduierung : datValidData;
            P_TrainngsDone      = Globals.DatabaseWrapper.NrOfTrainingsSince(P_memberId, P_dateStart);
            P_yearsOfMembership = DateTime.Now.Year - member.Eintritt.Value.Year;
        }

        public int CompareTo(BestGraduation other)
        {
            if (P_graduierung > other.P_graduierung)
                return -1;
            else if (P_graduierung < other.P_graduierung)
                return 1;
            else if (P_datumGraduierung < other.P_datumGraduierung)
                return -1;
            else if (P_datumGraduierung > other.P_datumGraduierung)
                return 1;
            else if (P_memberId < other.P_memberId)
                return -1;
            else
                return 1;
        }

        public void ReplaceIfBetter(int grad, DateTime date)
        {
            if (grad > P_graduierung)
            {
                P_graduierung      = grad;
                P_datumGraduierung = date;
            }
        }
    }

    public class BestGraduationList
    {
        public List<BestGraduation> P_listBestGraduation { get; private set; }

        public BestGraduationList()
        {
            P_listBestGraduation = new List<BestGraduation>();

            foreach (Mitglied member in Globals.DatabaseWrapper.CurrentMembers())
            {
                BestGraduation best = new BestGraduation(member);

                foreach (Pruefung pruefung in Globals.DatabaseWrapper.P_pruefung)
                {
                    if (pruefung.Pruefling == member.Id)
                    {
                        best.ReplaceIfBetter(pruefung.GraduierungID, pruefung.Datum);
                    }
                }

                best.Complete(member);
                P_listBestGraduation.Add(best);
            }

            P_listBestGraduation.Sort();
        }
    }
}
