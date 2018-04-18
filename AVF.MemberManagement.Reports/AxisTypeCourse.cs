using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeCourse : AxisType
    {
        public AxisTypeCourse(ReportDescriptor desc)
            : base(desc)
        { 
            P_ActiveElementsOnly = false;
            P_MaxDbId = Globals.DatabaseWrapper.MaxKursNr();
            P_MinDbId = Globals.DatabaseWrapper.MinKursNr();
            HeaderStrings = new List<string> { "Termin" };
        }

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.P_kurs.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => Globals.DatabaseWrapper.P_kurs[iModelIndex].Id;

        public override string MouseAxisEvent(int idKurs, bool action)
            => action
               ? ReportMain.P_formMain.NewTrainingsParticipationPanel
                 (
                     defaultDesc: P_reportDescriptor,
                     xAxisType: typeof(AxisTypeTraining), 
                     yAxisType: typeof(AxisTypeMember), 
                     idCourse: idKurs
                 )
               : $"Klicken für Details zum Kurs\n" + GetFullDesc(idKurs, Globals.TEXT_ORIENTATION.HORIZONTAL);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);

        public static List<string> GetDesc(int idKurs)
        {
            List<string> list = new List<string>();
            if (idKurs == Globals.ALL_COURSES)
            {
                list.Add("Alle Kurse");
                return list;
            }

            Kurs kurs = Globals.DatabaseWrapper.KursFromId(idKurs);

            if (kurs.Zeit == TimeSpan.Zero)
            {
                list.Add("_Lehrg. etc.");
                return list;
            }

            string day = Globals.DatabaseWrapper.WeekDay(kurs.WochentagID).Substring(0, 2);
            list.Add($"{ day } {kurs.Zeit:hh}:{kurs.Zeit:mm}");
            return list;
        }

        public override List<string> GetDescription(int idKurs, Globals.TEXT_ORIENTATION o)
            => GetDesc(idKurs);
   }
}
