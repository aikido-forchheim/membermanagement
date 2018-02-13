﻿using System.Windows.Forms;
using System.Globalization;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisWeeks : VerticalAxis
    {
        private DateTimeFormatInfo m_dfi;
        private Calendar m_cal;
        private const int LAST_CALENDAR_WEEK = 52;
        private const int FIRST_CALENDAR_WEEK = 1;

        public VerticalAxisWeeks()
        {
            NrOfLeadingElements = 1; 
            ActiveElementsOnly = false;
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
        }

        public override int MaxDatabaseId
            => LAST_CALENDAR_WEEK;   // Not really in database, but kind of

        public override int MinDatabaseId
            => FIRST_CALENDAR_WEEK;  // Not really in database, but kind of

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
        {
            Training training = Globals.DatabaseWrapper.TrainingFromId(tn.TrainingID);
            return m_cal.GetWeekOfYear(training.Termin, m_dfi.CalendarWeekRule, m_dfi.FirstDayOfWeek) - MinDatabaseId;
        }

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iDgvStartIndex)
            => FillMainHeaderCells(dgv, tpModel, iDgvStartIndex);

        protected override void FillHeaderCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow)
        {
            dgv[0, iDgvRow++].Value = iModelRow + MinDatabaseId;
        }
    }
}
