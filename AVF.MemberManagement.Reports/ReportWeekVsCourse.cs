using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportWeekVsCourse : ReportBase
    {
        private int? m_idMember;

        private const int NR_OF_CALENDAR_WEEKS = 52;

        public ReportWeekVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd, int ? idMember)
            : base(db, datStart, datEnd)
        {
            m_idMember = idMember;

            m_xAxis = new HorizontalAxisCourses(db, m_tpMatrix);
            m_yAxis = new VerticalAxisWeeks(db, m_tpMatrix);

            m_tpMatrix.Initialize
            (
                m_xAxis,
                m_yAxis,
                tn => m_idMember.Value == tn.MitgliedID
            );
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue > 0) ? $"{ iValue, -3 }" : "   ";
        }
    }
}
