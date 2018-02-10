using System;
using System.Drawing;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class ReportForm : Form
    {
        protected DateTime                   m_datStart { get; set; }
        protected DateTime                   m_datEnd   { get; set; }
        protected VerticalAxis               m_yAxis    { get; set; }
        protected HorizontalAxis             m_xAxis    { get; set; }
        protected TrainingParticipationModel m_tpModel  { get; set; }

        public ReportForm(DateTime datStart, DateTime datEnd)
        {
            InitializeComponent( );
            Load += new EventHandler(ReportFormLoad);

            m_datStart = datStart;
            m_datEnd   = datEnd;
        }

        private void ReportFormLoad(System.Object sender, EventArgs e)
        {
            m_dataGridView.RowCount = m_yAxis.GetNrOfDgvRows(m_tpModel) + m_xAxis.GetMrOfAdditionalElements();
            m_dataGridView.ColumnCount = m_xAxis.GetNrOfDgvCols(m_tpModel) + m_yAxis.GetMrOfAdditionalElements();

            m_xAxis.FillHeaderCells(m_dataGridView, m_tpModel, m_yAxis.NrOfLeadingElements);
            m_xAxis.FillSumCells   (m_dataGridView, m_tpModel, m_yAxis.NrOfLeadingElements);
            m_yAxis.FillHeaderCells(m_dataGridView, m_tpModel, m_xAxis.NrOfLeadingElements);
            m_yAxis.FillSumCells   (m_dataGridView, m_tpModel, m_xAxis.NrOfLeadingElements);

            int iDgvRow = 0;
            m_dataGridView.Rows[m_dataGridView.RowCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            m_tpModel.ForAllRows
            (
                iRow =>
                {
                    int iDgvCol = m_yAxis.NrOfLeadingElements;
                    m_tpModel.ForAllCols
                    (
                        iCol => m_dataGridView[iDgvCol++, iDgvRow].Value = FormatMatrixElement(m_tpModel.GetCell(iRow, iCol)),
                        m_xAxis.ActiveElementsOnly
                    );
                    ++iDgvRow;
                },
                m_yAxis.ActiveElementsOnly
            );

            Controls.Add(m_dataGridView);
            Size = new Size(1000, 500);
        }

        private string FormatMatrixElement(int iValue)
        {
            return (iValue > 0) ? $"{ iValue,-3 }" : "   ";
        }
    }
}
