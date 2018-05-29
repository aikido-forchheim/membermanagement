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
            P_exam = examThis;
            P_gradNext = Globals.DatabaseWrapper.GraduierungFromId(examThis.GraduierungID + 1);
            P_range    = new TimeRange(examThis.Datum, dateNextExam);
            P_memberId = examThis.Pruefling;
            initialize();
        }

        public Examination(Mitglied member, DateTime dateEnd, Graduierung gradNext)
        {
            P_exam = null;
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

        public string GraduationText()
            => (P_exam == null)
               ? $"Eintritt:"
               : $"{Globals.DatabaseWrapper.GraduierungFromId(P_exam.GraduierungID).Bezeichnung}";

        public string GraduationDate()
            => $"{((P_exam == null) ? P_range.P_datStart : P_exam.Datum):dd.MM.yyyy}";

        public string Examinant()
        {
            if (P_exam == null)
                return String.Empty;

            if (P_exam.Pruefer > 0)
            {
                Mitglied pruefer = Globals.DatabaseWrapper.MitgliedFromId(P_exam.Pruefer);
                return $"{ pruefer.Vorname } { pruefer.Nachname }";
            }
            else
                return $"{P_exam.Bemerkung}";
        }

        public string WaitTime()
            => (P_monthsTilNextExam > 0)
               ? $" {P_monthsTilNextExam,3}({P_waitTimeMonths})"
               : "        ";

        public string NrOfTrainings()
        {
            DateTime datValidData = Globals.DatabaseWrapper.GetStartValidData();
            string sTrainings;
            if (P_range.P_datEnd < datValidData)
                sTrainings = " ????";
            else
            {
                sTrainings = (P_range.P_datStart < datValidData) ? " >" : "  ";
                sTrainings += $"{ P_nrOfTrainingsSinceLastExam,3 }";
            }
            sTrainings += $" ({P_nrOfTrainingsNeeded})";
            return sTrainings;
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

        public List<Examination> GetBestGraduationList(Action<String> tick)   // list of highest graduations for each member
        {
            List<Examination> result = new List<Examination>();

            foreach (Mitglied member in Globals.DatabaseWrapper.ActiveMembers())
            {                
                tick($"{member.Vorname} {member.Nachname}");
                result.Add(GetSortedListOfExaminations(member)[0]);
            }

            tick("");
            result.Sort();
            return result;
        }
    }
}
