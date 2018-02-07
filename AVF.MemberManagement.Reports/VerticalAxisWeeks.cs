using System.Windows.Forms;
using System.Globalization;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisWeeks : VerticalAxis
    {
        private DateTimeFormatInfo m_dfi;
        private Calendar m_cal;
        private const int NR_OF_CALENDAR_WEEKS = 52;

        public VerticalAxisWeeks(DatabaseWrapper db, TrainingParticipationMatrix tpMatrix)
            : base(db, tpMatrix)
        {
            m_iNrOfColsOnLeftSide = 1; 
            m_activeRowsOnly = false;
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
        }

        public override int GetNrOfSrcElements()
            => NR_OF_CALENDAR_WEEKS;

        public override int GetIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => m_cal.GetWeekOfYear(m_db.TrainingFromId(tn.TrainingID).Termin, m_dfi.CalendarWeekRule, m_dfi.FirstDayOfWeek) - 1;

        public override void FillRowHeaderCols(DataGridView dgv)
        {
            int iDgvRow = 0;
            m_tpMatrix.ForAllRows(action: iRow => dgv[0, iDgvRow++].Value = iRow + 1, activeRowsOnly: m_activeRowsOnly);
        }

        public override void FillRowSumCols(DataGridView dgv)
        {
            FillRowSumCols(dgv, m_activeRowsOnly);
        }
    }
}
