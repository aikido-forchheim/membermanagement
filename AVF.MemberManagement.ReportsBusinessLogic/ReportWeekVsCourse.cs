using System;
using System.Globalization;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportBusinessLogic
{
    public class ReportWeekVsCourse : ReportBase
    {
        DateTimeFormatInfo m_dfi;
        Calendar           m_cal;
        int ?              m_idMember;

        public ReportWeekVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd, int ? idMember)
            : base(db, datStart, datEnd)
        {
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
            m_idMember = idMember;
        }

        protected override bool IsRelevant(TrainingsTeilnahme tn)
        {
            return ( m_idMember.HasValue ) ? (m_idMember.Value == tn.MitgliedID) :  true;
        }

        protected override int RowIndexFromTrainingParticipation(TrainingsTeilnahme tn)
        {
            DateTime date = m_db.TrainingFromId(tn.TrainingID).Termin;

            int iCalendarWeek = m_cal.GetWeekOfYear(date, m_dfi.CalendarWeekRule, m_dfi.FirstDayOfWeek );

            return iCalendarWeek - 1;
        }

        protected override int ColIndexFromTrainingParticipation(TrainingsTeilnahme tn)
        {
            int? idKurs = m_db.TrainingFromId(tn.TrainingID).KursID;
            return idKurs.HasValue ? (idKurs.Value - 1) : m_iNrOfCols - 1;
        }

        protected override void FillHeaderRows()
        {
            ReportDataGridView[0, 0].Value = "Kalender";
            ReportDataGridView[0, 1].Value = "woche   ";

            FillCourseHeaderRows();
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
            m_iNrOfColsOnLeftSide = 1;   // column for Mitglieder
            m_iNrOfColsOnRightSide = 1;  // column for row sum
            m_iNrOfHeaderRows = 3;       // one empty row

            Initialize
            (
                52,                    // Number of calendar weeks in year
                m_db.MaxKursNr() + 1   // One additional col for "Lehrgänge"
            );

            CollectData();

            FillHeaderRows();
            FillMainRows();
            FillFooterRow("Summe  ");

            return ReportDataGridView;
        }
    }
}
