using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourses : ReportForm
    {
        private DateTime m_datStart { get; set; }
        private DateTime m_datEnd   { get; set; }

        public ReportMemberVsCourses( DateTime datStart, DateTime datEnd )
        {
            m_xAxis = new HorizontalAxisCourses( );
            m_yAxis = new VerticalAxisMembers( );

            m_xAxis.StartIndex = VerticalAxis.NrOfLeadingElements;
            m_yAxis.StartIndex = HorizontalAxis.NrOfLeadingElements;

            m_tpModel = new TrainingParticipationModel
            (
                datStart,
                datEnd,
                m_xAxis,
                m_yAxis,
                filter: tn => true
            );

            m_datStart = datStart;
            m_datEnd = datEnd;

            m_label1.Text = "Überblick Trainingsteilnahme";
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
            m_dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            m_dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);
        }

        private void CellMouseEnter(object sender, DataGridViewCellEventArgs e)
            => ToolTip(e, true);

        private void CellMouseLeave(object sender, DataGridViewCellEventArgs e)
            => ToolTip(e, false);

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
                    int idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                    newForm = new ReportWeekVsCourses(m_datStart, m_datEnd, idMember);
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

        private void ToolTip(DataGridViewCellEventArgs e, bool showTip)
        {
            DataGridViewCell cell =
                RowIsHeader(e.RowIndex)
                ? m_dataGridView.Columns[e.ColumnIndex].HeaderCell
                : m_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (!showTip)
            {
                cell.ToolTipText = String.Empty;
                return;
            }

            if (ColIsKeyArea(e.ColumnIndex) || ColIsSummary(e.ColumnIndex))
            {
                if (RowIsHeader(e.RowIndex) || RowIsFooter(e.RowIndex))
                {

                }
                else
                {
                    int      idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                    Mitglied member   = Globals.DatabaseWrapper.MitgliedFromId(idMember);
                    cell.ToolTipText  = $"Klicken für Details zu Mitglied\n {member.Vorname} {member.Nachname}";
                }
            }
            else // Main area column
            {
                cell.ToolTipText = $"Klicken für Details zu diesem Kurs";
            }
        }
    }
}
