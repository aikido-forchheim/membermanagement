using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTrainings : ReportForm
    {
        public ReportMemberVsTrainings(DateTime datStart, DateTime datEnd, int idKurs)
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
            => (iValue > 0) ? $" X " : " ";

        private void CellMouseEnter(object sender, DataGridViewCellEventArgs e)
            => ToolTip(e, true);

        private void CellMouseLeave(object sender, DataGridViewCellEventArgs e)
            => ToolTip(e, false);

        private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            ReportForm newForm = null;

            if (ColIsKeyArea(e.ColumnIndex) || ColIsSummary(e.ColumnIndex))
            {
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
                    int idTraining = m_xAxis.GetDbId(e.ColumnIndex - m_xAxis.StartIndex);
                    Training trainng = Globals.DatabaseWrapper.TrainingFromId(idTraining);
                    cell.ToolTipText = $"Klicken für Details zu diesem Training";
                }
            }
            else // Main area column
            {
            }
        }
    }
}
