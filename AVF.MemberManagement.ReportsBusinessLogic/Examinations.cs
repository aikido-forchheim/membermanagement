using System;
using System.Linq;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Examination
    {
        public Examination(Pruefung examThis, DateTime dateNextExam)
        {
            P_gradNext = Globals.DatabaseWrapper.GraduierungFromId(examThis.GraduierungID + 1);
            P_range = new TimeRange(examThis.Datum, dateNextExam);
            P_memberId = examThis.Pruefling;
            P_exam     = examThis;
            initialize();
        }

        public Examination(int idMember, TimeRange range, Graduierung gradNext)
        {
            P_memberId = idMember;
            P_range = range;
            P_gradNext = gradNext;
            P_exam = null;
            initialize();
        }

        public void initialize()
        {
            P_nrOfTrainingsSinceLastExam = Globals.DatabaseWrapper.NrOfTrainingsInRange(P_memberId, P_range);
            P_monthsTilNextExam          = P_range.NrOfMonthsInRange();
            P_waitTimeMonths             = Graduation.WaitTimeMonths(P_gradNext);
            P_nrOfTrainingsNeeded        = Graduation.TrainingsNeeded(P_gradNext);
            P_gradId                     = P_gradNext.Id - 1;
        }

        public Pruefung    P_exam                       { get; private set; }
        public int         P_monthsTilNextExam          { get; private set; }
        public int         P_nrOfTrainingsSinceLastExam { get; private set; }
        public int         P_waitTimeMonths             { get; private set; }
        public int         P_nrOfTrainingsNeeded        { get; private set; }
        public TimeRange   P_range                      { get; private set; }
        public int         P_memberId                   { get; private set; }
        public Graduierung P_gradNext                   { get; private set; }
        public int         P_gradId                     { get; private set; }
    }

    public class Examinations
    {
        public List<Examination> GetSortedListOfExaminations( Mitglied member )
        {
            var pruefungen = Globals.DatabaseWrapper.P_pruefung.Where(p => p.Pruefling == member.Id).OrderByDescending(x => x.GraduierungID).ToList();

            List<Examination> result = new List<Examination>();

            DateTime dateNext = DateTime.Now;

            foreach (Pruefung p in pruefungen)
            {
                result.Add(new Examination(p, dateNext));
                dateNext = p.Datum;
            }

            Graduierung gradNext;

            if (pruefungen.Count > 0)
            {
                dateNext = pruefungen.Last().Datum;
                gradNext = Globals.DatabaseWrapper.GraduierungFromId(pruefungen.Last().GraduierungID);
            }
            else
            {
                dateNext = DateTime.Now;
                gradNext = Globals.DatabaseWrapper.GraduierungFromId(8);   // 5. Kyu
            }
            TimeRange range = new TimeRange(member.Eintritt.Value, dateNext);
            result.Add(new Examination(member.Id, range, gradNext));

            return result;
        }
    }
}
