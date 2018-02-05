using System;
using System.Globalization;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportWeekVsCourse : ReportBase
    {
        private DateTimeFormatInfo m_dfi;
        private Calendar           m_cal;
        private int?               m_idMember;

        private const int NR_OF_CALENDAR_WEEKS = 52;

        public ReportWeekVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd, int ? idMember)
            : base(db, datStart, datEnd)
        {
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
            m_idMember = idMember;

            m_iNrOfColsOnLeftSide  = 1;  // 1 columns for week number
            m_iNrOfColsOnRightSide = 1;  // 1 column for row sum

            m_coreReport.Initialize
            (
                datStart,
                datEnd,
                NR_OF_CALENDAR_WEEKS,              
                m_db.MaxKursNr() + 1,   // One additional col for "Lehrgänge"
                tn => ( m_idMember.HasValue ) ? (m_idMember.Value == tn.MitgliedID) :  true,
                tn => m_cal.GetWeekOfYear(m_db.TrainingFromId(tn.TrainingID).Termin, m_dfi.CalendarWeekRule, m_dfi.FirstDayOfWeek) - 1,
                tn => m_db.KursIdFromTrainingId(tn.TrainingID)
            );
        }

        protected override void SizeDataGridView(DataGridView dgv)
        {
            dgv.RowCount    = NR_OF_CALENDAR_WEEKS + 1;  // one footer row
            dgv.ColumnCount = m_db.MaxKursNr() + 1 + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;
        }

        protected override void FillHeaderRows(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Kalender\nwoche";

            FillCourseHeaderRows(dgv);
        }

        protected override void FillRowHeaderColumns(DataGridView dgv)
        {
            int iDgvRow = 0;
            m_coreReport.ForAllRows( action: iRow => dgv[0, iDgvRow++].Value = iRow + 1, activeRowsOnly: false );
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue > 0) ? $"{ iValue, -3 }" : "   ";
        }
    }
}
