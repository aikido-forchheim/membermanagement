﻿using System;
using System.Globalization;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportBusinessLogic
{
    public class ReportWeekVsCourse : ReportBase
    {
        private DataGridView m_dataGridView = new DataGridView();

        private DateTimeFormatInfo m_dfi;
        private Calendar           m_cal;
        private int?               m_idMember;

        public ReportWeekVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd, int ? idMember)
            : base(db, datStart, datEnd)
        {
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
            m_idMember = idMember;

            m_iNrOfColsOnLeftSide = 1;   // column for Mitglieder
            m_iNrOfColsOnRightSide = 1;  // column for row sum
            m_iNrOfHeaderRows = 3;       // one empty row

            Initialize
            (
                52,                    // Number of calendar weeks in year
                m_db.MaxKursNr() + 1   // One additional col for "Lehrgänge"
            );

            CollectData
            (
                tn => ( m_idMember.HasValue ) ? (m_idMember.Value == tn.MitgliedID) :  true,
                tn => m_cal.GetWeekOfYear(m_db.TrainingFromId(tn.TrainingID).Termin, m_dfi.CalendarWeekRule, m_dfi.FirstDayOfWeek) - 1,
                tn => 
                {
                    int idKurs = m_db.KursIdFromTrainingId(tn.TrainingID);
                    return ((idKurs == 0) ? idKurs : m_iNrOfCols) - 1;
                }
            );
        }

        protected override void FillHeaderRows()
        {
            m_dataGridView[0, 0].Value = "Kalender";
            m_dataGridView[0, 1].Value = "woche   ";

            FillCourseHeaderRows(m_dataGridView);
        }

        protected override string FormatFirstColElement(int iRow)
        {
            return Utilities.FormatNumber(iRow + 1);
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return Utilities.FormatNumber(iValue);
        }

        protected override string FormatColSumElement(int iValue)
        {
            return Utilities.FormatNumber(iValue);
        }

        public DataGridView GetMatrix()
        {
            m_dataGridView.RowCount = GetNrOfRows();  // one footer row
            m_dataGridView.ColumnCount = GetNrOfCols();

            FillHeaderRows();
            FillMainRows(m_dataGridView);
            FillFooterRow(m_dataGridView, "Summe  ");

            return m_dataGridView;
        }
    }
}
