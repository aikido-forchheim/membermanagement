using System;
using System.Linq;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Examination : IComparable<Examination>
    {
        public Examination(Pruefung examThis, DateTime dateNextExam)
        {
            P_gradNext = Globals.DatabaseWrapper.GraduierungFromId(examThis.GraduierungID + 1);
            P_range    = new TimeRange(examThis.Datum, dateNextExam);
            P_memberId = examThis.Pruefling;
            initialize();
        }

        public Examination(Mitglied member, DateTime dateEnd, Graduierung gradNext)
        {
            P_memberId = member.Id;
            P_range = new TimeRange(member.Eintritt.Value, dateEnd);
            P_gradNext = gradNext;
            initialize();
        }

        public void initialize()
        {
            P_nrOfTrainingsSinceLastExam = Globals.DatabaseWrapper.NrOfTrainingsInRange(P_memberId, P_range);
            P_monthsTilNextExam          = P_range.NrOfMonthsInRange();
            P_waitTimeMonths             = P_gradNext.WartezeitJahre * 12 + P_gradNext.WartezeitMonate;
            P_nrOfTrainingsNeeded        = (P_waitTimeMonths * 100) / 12;
            P_gradId                     = P_gradNext.Id - 1;
            P_datumMinNextGrad           = P_range.P_datStart.AddYears(P_gradNext.WartezeitJahre).AddMonths(P_gradNext.WartezeitMonate);
        }

        public int CompareTo(Examination other)
        {
            if (P_gradId > other.P_gradId)
                return -1;
            else if (P_gradId < other.P_gradId)
                return 1;
            else if (P_range.P_datStart < other.P_range.P_datStart)
                return -1;
            else if (P_range.P_datStart > other.P_range.P_datStart)
                return 1;
            else if (P_memberId < other.P_memberId)
                return -1;
            else
                return 1;
        }

        public int         P_monthsTilNextExam          { get; private set; }
        public int         P_nrOfTrainingsSinceLastExam { get; private set; }
        public int         P_waitTimeMonths             { get; private set; }
        public int         P_nrOfTrainingsNeeded        { get; private set; }
        public TimeRange   P_range                      { get; private set; }
        public int         P_memberId                   { get; private set; }
        public Graduierung P_gradNext                   { get; private set; }
        public int         P_gradId                     { get; private set; }
        public DateTime    P_datumMinNextGrad           { get; private set; }
    }

    public class Examinations
    {
        public List<Examination> GetSortedListOfExaminations( Mitglied member )  // highest graduation first
        {
            List<Pruefung>    pruefungen = Globals.DatabaseWrapper.P_pruefung.Where(p => p.Pruefling == member.Id).OrderByDescending(x => x.GraduierungID).ToList();
            List<Examination> result     = new List<Examination>();

            DateTime dateNext = DateTime.Now;

            foreach (Pruefung p in pruefungen)
            {
                result.Add(new Examination(p, dateNext));
                dateNext = p.Datum;
            }

             // add pseudo examination for AVF entry

            if (pruefungen.Count > 0)
            {
                result.Add(new Examination(member, pruefungen.Last().Datum, Globals.DatabaseWrapper.GraduierungFromId(pruefungen.Last().GraduierungID)));
            }
            else
            {
                result.Add(new Examination(member, DateTime.Now, Globals.DatabaseWrapper.GraduierungFromId(8)));
            }

            return result;
        }

        public List<Examination> GetBestGraduationList()   // list of highest graduations for each member
        {
            List<Examination> result = new List<Examination>();

            foreach (Mitglied member in Globals.DatabaseWrapper.CurrentMembers())
            {
                result.Add(GetSortedListOfExaminations(member)[0]);
            }

            result.Sort();
            return result;
        }
    }
}
