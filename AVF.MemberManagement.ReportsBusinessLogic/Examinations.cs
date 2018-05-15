using System;
using System.Linq;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Examination
    {
        public Examination(Pruefung examThis, DateTime dateStart)
        {
            TimeRange   range            = new TimeRange(dateStart, examThis.Datum);
            Graduierung grad             = Globals.DatabaseWrapper.GraduierungFromId(examThis.GraduierungID);
            P_waitTimeMonths             = Graduation.WaitTimeMonths(grad);
            P_nrOfTrainingsNeeded        = Graduation.TrainingsNeeded(grad);
            P_monthsSinceLastExam        = range.NrOfMonthsInRange();
            P_nrOfTrainingsSinceLastExam = Globals.DatabaseWrapper.NrOfTrainingsInRange(examThis.Pruefling, range);
            P_dateLastExam               = dateStart;
            P_exam                       = examThis;
        }

        public Pruefung P_exam                       { get; private set; }
        public int      P_monthsSinceLastExam        { get; private set; }
        public int      P_nrOfTrainingsSinceLastExam { get; private set; }
        public int      P_waitTimeMonths             { get; private set; }
        public int      P_nrOfTrainingsNeeded        { get; private set; }
        public DateTime P_dateLastExam               { get; private set; }
    }

    public class Examinations
    {
        public List<Examination> GetSortedListOfExaminations( Mitglied member )
        {
            var examinations = Globals.DatabaseWrapper.P_pruefung.Where(p => p.Pruefling == member.Id).OrderBy(x => x.GraduierungID).ToList();

            List<Examination> result = new List<Examination>();

            DateTime dateStart = member.Eintritt.Value;

            foreach (Pruefung exam in examinations)
            {
                result.Add(new Examination(exam, dateStart));
                dateStart = exam.Datum;
            }

            result.Reverse();
            return result;
        }
    }
}
