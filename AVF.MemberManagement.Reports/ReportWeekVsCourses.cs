using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeekVsCourses : ReportForm
    {
        public ReportWeekVsCourses(DateTime datStart, DateTime datEnd, int idMember)
            : base( datStart, datEnd )
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

            m_label1.Text  = "Trainingsteilnahme " + Globals.GetMemberDescription( idMember );
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
            m_dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            m_dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);
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
                int idKurs = m_xAxis.GetDbId(e.ColumnIndex - m_xAxis.StartIndex);
                newForm = new ReportMemberVsTrainings(m_datStart, m_datEnd, idKurs);
            }

            if (newForm != null)
                newForm.Show();
        }

        protected override string ToolTipText(DataGridViewCellEventArgs e)
        {
            if (ColIsKeyArea(e.ColumnIndex) || ColIsSummary(e.ColumnIndex))
            {
                if (RowIsHeader(e.RowIndex) || RowIsFooter(e.RowIndex))
                {
                }
                else // Main area column
                {
                    return $"Klicken für Details zu Trainings in dieser Woche (Noch nicht implementiert)";
                }
            }
            else // Main area column
            {
                if (RowIsHeader(e.RowIndex) )
                {
                    return $"Klicken für Details zu diesem Kurs";
                }
                else if ( RowIsFooter(e.RowIndex) )
                {
                }
                else // Main area column
                {
                    return $"Training ...";
                }
            }
            return String.Empty;
        }
    }
}
