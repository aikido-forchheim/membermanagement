using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourse : ReportForm
    {
        public ReportMemberVsCourse( DateTime datStart, DateTime datEnd )
            : base(datStart, datEnd )
        {
            m_xAxis = new HorizontalAxisCourses();
            m_yAxis = new VerticalAxisMembers();

            m_tpModel = new TrainingParticipationModel
            (
                datStart,
                datEnd,
                m_xAxis,
                m_yAxis,
                filter: tn => true
            );

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
        }

        protected void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            bool isOnMainAreaRow = (0 <= e.RowIndex) && (e.RowIndex < m_dataGridView.RowCount);
            bool IsOnMainAreaCol = (m_yAxis.NrOfLeadingElements <= e.ColumnIndex) && (e.ColumnIndex < m_dataGridView.ColumnCount);

            if (isOnMainAreaRow)
            {
                if (IsOnMainAreaCol)
                {
                    int idKurs = (e.ColumnIndex == m_yAxis.NrOfLeadingElements) ? 0 : e.ColumnIndex - m_yAxis.NrOfLeadingElements;
                    var newForm = new ReportMemberVsTraining(m_datStart, m_datEnd, idKurs);
                    newForm.Show();
                }
                else // row header or row sum
                {
                    int idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                    var newForm = new ReportWeekVsCourse(m_datStart, m_datEnd, idMember);
                    newForm.Show();
                }
            }
            else  // header or footer 
            {
                if (IsOnMainAreaCol)
                {
                    int idKurs = e.ColumnIndex - m_yAxis.NrOfLeadingElements;
                    var newForm = new ReportMemberVsTraining(m_datStart, m_datEnd, idKurs);
                    newForm.Show();
                }
                else
                {

                }
            }
        }

    }
}
