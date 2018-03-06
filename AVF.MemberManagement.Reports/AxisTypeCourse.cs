﻿using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeCourse : AxisType
    {
        public AxisTypeCourse(DateTime datStart, DateTime datEnd)
            : base( datStart, datEnd)
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
               ? ReportMain.SwitchToPanel(new ReportMemberVsTrainings(P_datStart, P_datEnd, idKurs))
               : $"Klicken für Details zum Kurs\n" + GetDescription(idKurs);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID);

        public override string GetDescription(int idKurs, int iNr = 1)
        {
            Kurs kurs = Globals.DatabaseWrapper.KursFromId(idKurs);

            if (kurs.Zeit == TimeSpan.Zero)
            {
                return $"_Lehrg. etc.";
            }
            else
            {
                string day = Globals.DatabaseWrapper.WeekDay(kurs.WochentagID).Substring(0, 2);
                return $"{ day } {kurs.Zeit:hh}:{kurs.Zeit:mm}";
            }
        }
    }
}
