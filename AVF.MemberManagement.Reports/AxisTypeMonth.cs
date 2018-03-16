﻿using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMonth : AxisType
    {
        public AxisTypeMonth(DateTime datStart, DateTime datEnd)
             : base(datStart, datEnd)
        {
            P_ActiveElementsOnly = false;
            P_MinDbId = 0;
            P_MaxDbId = NrOfMonths(datStart, datEnd);
            HeaderStrings = new List<string> { "Monat" };
        }

        private int NrOfMonths(DateTime datStart, DateTime datEnd)
            => (datEnd.Year - datStart.Year) * 12 + (datEnd.Month - datStart.Month);

        public override int GetModelIndexFromId(int id)
            => id;

        public override int GetIdFromModelIndex(int iModelIndex)
            => iModelIndex;

        public override string MouseAxisEvent(int idMonth, bool action)
            => action
               ? ReportMain.P_formMain.SwitchToPanel(new ReportWeeksVsCourses(Globals.FirstDayOfMonth(P_datStart, idMonth), Globals.LastDayOfMonth(P_datStart, idMonth)))
               : $"Klicken für Details zum Monat " + GetDescription(idMonth);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => NrOfMonths(P_datStart, Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int idMonth, int iNr = 1)
        {
            if (idMonth < 0)
            {
                return "Alle Monate";
            }
            else
            {
                int nrOfMonths = (P_datStart.Month - 1) + idMonth;
                int year = P_datStart.Year + nrOfMonths / 12;
                string strMonat = Globals.GetMonthName(nrOfMonths % 12 + 1);
                string strMonthShort = strMonat.Substring(0, 3);
                return $"{strMonthShort} {year}";
            }
        }
    }
}
