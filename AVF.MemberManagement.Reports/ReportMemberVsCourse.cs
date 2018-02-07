using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportMemberVsCourse : ReportBase
    {
        public ReportMemberVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
            : base(db, datStart, datEnd )
        {
            m_xAxis = new HorizontalAxisCourses(db, m_tpMatrix);
            m_yAxis = new VerticalAxisMembers(db, m_tpMatrix);

            m_tpMatrix.Initialize
            (
                m_xAxis,
                m_yAxis,
                tn => true
            );

            m_tpMatrix.SortRows();
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue > 0) ? $"{ iValue, -3 }" : "   ";
        }
    }
}
