using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourses : ReportForm
    {
        public ReportMemberVsCourses( DateTime datStart, DateTime datEnd )
        {
            CreateModel
            (
                datStart, datEnd,
                new HorizontalAxisCourses(), 
                new VerticalAxisMembers(),
                filter: tn => true
            );

            m_label1.Text = "Überblick Trainingsteilnahme";
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }

        protected override string MouseHeaderCellEvent(int col, bool action)
            => MouseCourseHeaderCellEvent(col, action);

        protected override string MouseKeyCellEvent(int row, bool action)
            => MouseMemberKeyCellEvent(row, action);

        protected override string MouseMainDataAreaCellEvent( int row, int col, bool action )
        {
            int idMember = m_yAxis.GetRowKey(m_dataGridView, row);

            if (action)
            {
                ReportForm newForm = new ReportMemberVsTrainings(m_datStart, m_datEnd, m_xAxis.GetDbId(col));
                newForm.Show();
                return String.Empty;
            }
            else
            {
                return $"Klicken für Details zur Teilnahme von\n" + Globals.GetMemberDescription( idMember ) + $" am Kurs\n" + Globals.GetCourseDescription(m_xAxis.GetDbId(col));
            }
        }
    }
}
