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
            // define dimensions of DataGridView

            m_dataGridView.RowCount    = m_yAxis.GetNrOfDgvRows(m_tpModel) + m_xAxis.GetMrOfAdditionalElements();
            m_dataGridView.ColumnCount = m_xAxis.GetNrOfDgvCols(m_tpModel) + m_yAxis.GetMrOfAdditionalElements();

            // Fill cells of DataGridView

            m_xAxis.FillHeaderCells(m_dataGridView, m_tpModel, m_yAxis.NrOfLeadingElements);
            m_xAxis.FillSumCells   (m_dataGridView, m_tpModel, m_yAxis.NrOfLeadingElements);
            m_yAxis.FillHeaderCells(m_dataGridView, m_tpModel, m_xAxis.NrOfLeadingElements);
            m_yAxis.FillSumCells   (m_dataGridView, m_tpModel, m_xAxis.NrOfLeadingElements);

            // Fill main area of DataGridView

            int iDgvRow = m_xAxis.NrOfLeadingElements;
            m_tpModel.ForAllRows
            (
                action: iModelRow =>
                {
                    int iDgvCol = m_yAxis.NrOfLeadingElements;
                    m_tpModel.ForAllCols
                    (
                        action: iModelCol => m_dataGridView[iDgvCol++, iDgvRow].Value = FormatMatrixElement(m_tpModel.GetCell(iModelRow, iModelCol)),
                        activeColsOnly: m_xAxis.ActiveElementsOnly
                    );
                    ++iDgvRow;
                },
                activeRowsOnly: m_yAxis.ActiveElementsOnly
            );

            m_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            m_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            m_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            Controls.Add(m_dataGridView);
            Size = new Size(1000, 500);
        }

        private string FormatMatrixElement(int iValue)
        {
            return (iValue > 0) ? $"{ iValue, -3 }" : "   ";
        }
    }
}

