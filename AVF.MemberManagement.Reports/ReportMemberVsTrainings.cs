using System;
using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTrainings : ReportForm
    {
        public ReportMemberVsTrainings(DateTime datStart, DateTime datEnd, int idKurs)
            : base(datStart, datEnd)
        {
            m_xAxis = new HorizontalAxisTrainings();
            m_yAxis = new VerticalAxisMembers();

            m_xAxis.StartIndex = VerticalAxis.NrOfLeadingElements;
            m_yAxis.StartIndex = HorizontalAxis.NrOfLeadingElements;

            m_tpModel = new TrainingParticipationModel
            (
                datStart,
                datEnd,
                m_xAxis,
                m_yAxis,
                (tn => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID) == idKurs)
            );

            m_label1.Text  = Globals.GetCourseDescription(idKurs);
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
            m_dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            m_dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? $"X" : " ";

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
                if (RowIsHeader(e.RowIndex))
                {
                    int idTraining = m_xAxis.GetDbId(e.ColumnIndex - m_xAxis.StartIndex);
                    newForm = new ReportTraining(idTraining);
                }
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
                else
                {
                    int idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                    Mitglied member = Globals.DatabaseWrapper.MitgliedFromId(idMember);
                    return $"Klicken für Details zu Mitglied\n {member.Vorname} {member.Nachname}";
                }
            }
            else // Main area column
            {
                if ( RowIsHeader(e.RowIndex) )
                {
                    Debug.Assert(e.ColumnIndex >= m_xAxis.StartIndex);
                    int idTraining = m_xAxis.GetDbId(e.ColumnIndex - m_xAxis.StartIndex);
                    Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
                    return $"Klicken für Details zu diesem Training";
                }
            }
            return String.Empty;
        }
    }
}
