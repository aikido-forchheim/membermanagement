using System;
//using System.Data;
using System.Linq;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.BusinessLogic
{
    public class Examination
    {
        public Examination(Pruefung examThis)
        {
            exam = examThis;
            yearsSinceLastExam = 0;
            monthsSinceLastExam = 0;
        }

        public Examination(Pruefung examThis, Pruefung examLast)
        {
            exam = examThis;

            TimeSpan diff = exam.Datum - examLast.Datum;
            /*
                        int monthDiff = System.Data.Linq.SqlClient.SqlMethods.DateDiffMonth(examLast.Datum, exam.Datum);
                        int dayDiff = System.Data.Linq.SqlClient.SqlMethods.DateDiffDay(startDT, exam.Datum);
                        int yearDiff = System.Data.Linq.SqlClient.SqlMethods.DateDiffYear(startDT, exam.Datum);
                        yearsSinceLastExam = diff.
            */
            yearsSinceLastExam = 0;
            monthsSinceLastExam = 0;
        }

        public Pruefung exam { get; private set; }
        public int yearsSinceLastExam { get; private set; }
        public int monthsSinceLastExam { get; private set; }
    }

    public static class Examinations
    {
        public static Examination[] GetListOfExaminations(DatabaseWrapper db, Mitglied member )
        {
            var examinations = new List<Pruefung>();

            foreach (Pruefung pruefung in db.m_pruefung)
            {
                if (pruefung.Pruefling == member.Id)
                {
                    examinations.Add(pruefung);
                }
            }

            examinations = examinations.OrderBy(x => x.GraduierungID).ToList();

            int nrOfExams = examinations.Count();
            Examination[] result = new Examination[nrOfExams];

            // Fill result array with examinations in reverse order, highest graduation first

            int index = nrOfExams-1;

            foreach (Pruefung exam in examinations)
            {
                result[index] = ( index == nrOfExams - 1) 
                                ? new Examination( exam ) 
                                : new Examination( exam, result[index+1].exam );
                --index;
            }

            return result;
        }
    }
}
