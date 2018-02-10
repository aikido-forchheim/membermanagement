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

        public VerticalAxisWeeks()
        {
            NrOfLeadingElements = 1; 
            ActiveElementsOnly = false;
            m_dfi = DateTimeFormatInfo.CurrentInfo;
            m_cal = m_dfi.Calendar;
        }

        public override int NrOfSrcElements => NR_OF_CALENDAR_WEEKS;

        public override int GetIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => m_cal.GetWeekOfYear(Globals.DatabaseWrapper.TrainingFromId(tn.TrainingID).Termin, m_dfi.CalendarWeekRule, m_dfi.FirstDayOfWeek) - 1;

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iStartIndex)
            => FillMainHeaderCells(dgv, tpModel, iStartIndex);

        protected override void FillHeaderCell(TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iRow)
        {
            dgv[0, iDgvRow++].Value = iRow + 1;
        }
    }
}
