using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Graduation
    {
        public static int WaitTimeMonths(Graduierung grad)
            => grad.WartezeitJahre * 12 + grad.WartezeitMonate;

        public static int TrainingsNeeded(Graduierung grad)
            => (WaitTimeMonths(grad) * 100) / 12;

        public static DateTime MinDateGradNext(Graduierung gradNext, DateTime dateGrad)
            => dateGrad.AddYears(gradNext.WartezeitJahre).AddMonths(gradNext.WartezeitMonate);
    }

    public class BestGraduation : IComparable<BestGraduation>
    {
        public int      P_memberId            { get; private set; }
        public int      P_graduierung         { get; private set; }
        public DateTime P_dateStart           { get; private set; }
        public DateTime P_datumGraduierung    { get; private set; }
        public DateTime P_datumMinNextGrad    { get; private set; }
        public int      P_nrOfTrainingsNeeded { get; private set; }
        public bool     P_fAllTrainingsInDb   { get; private set; }
        public int      P_TrainingsDone       { get; private set; }
        public int      P_yearsOfMembership   { get; private set; }

        public BestGraduation(Mitglied member, Examination ex)
        {
            DateTime datValidData = Globals.DatabaseWrapper.GetStartValidData();
            Graduierung gradNext = Globals.DatabaseWrapper.GraduierungFromId(ex.P_exam.GraduierungID + 1);

            P_memberId            = member.Id;
            P_graduierung         = ex.P_exam.GraduierungID;
            P_datumGraduierung    = ex.P_exam.Datum;
            P_datumMinNextGrad    = Graduation.MinDateGradNext(gradNext, P_datumGraduierung); 
            P_nrOfTrainingsNeeded = Graduation.TrainingsNeeded(gradNext); 
            P_fAllTrainingsInDb   = (datValidData <= P_datumGraduierung);
            P_dateStart           = P_fAllTrainingsInDb ? P_datumGraduierung : datValidData;
            P_TrainingsDone       = Globals.DatabaseWrapper.NrOfTrainingsSince(P_memberId, P_dateStart);
            P_yearsOfMembership   = DateTime.Now.Year - member.Eintritt.Value.Year;
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
    }

    public class BestGraduationList
    {
        public List<BestGraduation> P_listBestGraduation { get; private set; }

        public BestGraduationList()
        {
            P_listBestGraduation = new List<BestGraduation>();

            foreach (Mitglied member in Globals.DatabaseWrapper.CurrentMembers())
            {
                var examinations = new Examinations().GetSortedListOfExaminations(member);
                if (examinations.Count > 0)
                {
                    P_listBestGraduation.Add(new BestGraduation(member, examinations[0]));
                }
            }

            P_listBestGraduation.Sort();
        }
    }
}
