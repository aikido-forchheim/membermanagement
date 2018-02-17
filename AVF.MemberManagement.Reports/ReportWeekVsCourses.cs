using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeekVsCourses : ReportForm
    {
        public ReportWeekVsCourses(DateTime datStart, DateTime datEnd, int idMember)
        {
            m_xAxis = new HorizontalAxisCourses();
            m_yAxis = new VerticalAxisWeeks();

            m_xAxis.StartIndex = VerticalAxis.NrOfLeadingElements;
            m_yAxis.StartIndex = HorizontalAxis.NrOfLeadingElements;

            m_tpModel = new TrainingParticipationModel
            (
                datStart,
                datEnd,
                m_xAxis,
                m_yAxis,
                (tn => idMember == tn.MitgliedID)
            );

            m_label1.Text  = Globals.GetMemberDescription( idMember );
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
        }

        private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            ReportForm newForm = null;

            if (ColIsKeyArea(e.ColumnIndex) || ColIsSummary(e.ColumnIndex))
            {
                if (RowIsHeader(e.RowIndex) || RowIsFooter(e.RowIndex))
                {

                }
                else 
                {
                    // Report "Week", MemeberVsCourses for one week 
                }
            }
            else // Main area column
            {
            }

            if (newForm != null)
                newForm.Show();
        }
    }
}
