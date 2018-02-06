using System;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportMemberVsCourse : ReportBase
    {
        public ReportMemberVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
            : base(db, datStart, datEnd )
        {
            m_iNrOfColsOnLeftSide  = 3;   // 3 columns for Mitglieder
            m_iNrOfColsOnRightSide = 1;   // 1 column for row sum

            m_xAxis = new HorizontalAxisCourses(db, m_coreReport);
            m_yAxis = new VerticalAxisMembers(db, m_coreReport);

            m_coreReport.Initialize
            (
                m_xAxis,
                m_yAxis,
                tn => true
            );

            m_coreReport.SortRows();
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue > 0) ? $"{ iValue, -3 }" : "   ";
        }
    }
}
