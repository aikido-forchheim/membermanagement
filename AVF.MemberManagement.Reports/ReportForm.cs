using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract partial class ReportForm : Form
    {
        protected VerticalAxis               m_yAxis   { get; set; }
        protected HorizontalAxis             m_xAxis   { get; set; }
        protected TrainingParticipationModel m_tpModel { get; set; }

        protected DateTime m_datStart { get; set; }
        protected DateTime m_datEnd { get; set; }

        private void ReportFormConstructorCore()
        {
            InitializeComponent();
            Load += new EventHandler(ReportFormLoad);
        }

        public ReportForm()
        {
            ReportFormConstructorCore();
        }

        public ReportForm(DateTime datStart, DateTime datEnd)
        {
            m_datStart = datStart;
            m_datEnd = datEnd;
            ReportFormConstructorCore();
        }

        private void ReportFormLoad(System.Object sender, EventArgs e)
        {
            // define dimensions of DataGridView

            m_dataGridView.RowCount    = m_yAxis.GetNrOfDgvRows(m_tpModel) + m_xAxis.GetNrOfAdditionalElements();
            m_dataGridView.ColumnCount = m_xAxis.GetNrOfDgvCols(m_tpModel) + m_yAxis.GetNrOfAdditionalElements();

            // Fill cells of DataGridView

            m_xAxis.FillHeaderCells(m_dataGridView, m_tpModel);
            m_xAxis.FillSumCells   (m_dataGridView, m_tpModel);
            m_yAxis.FillHeaderCells(m_dataGridView, m_tpModel);
            m_yAxis.FillSumCells   (m_dataGridView, m_tpModel);

            // Fill main area of DataGridView

            int iDgvRow = HorizontalAxis.NrOfLeadingElements;
            m_tpModel.ForAllRows
            (
                action: iModelRow =>
                {
                    int iDgvCol = VerticalAxis.NrOfLeadingElements;
                    m_tpModel.ForAllCols
                    (
                        action: iModelCol => m_dataGridView[iDgvCol++, iDgvRow].Value = FormatMatrixElement(m_tpModel.GetCell(iModelRow, iModelCol)),
                        activeColsOnly: m_xAxis.ActiveElementsOnly
                    );
                    ++iDgvRow;
                },
                activeRowsOnly: m_yAxis.ActiveElementsOnly
            );

            m_dataGridView.AutoSizeColumnsMode         = DataGridViewAutoSizeColumnsMode.AllCells;
            m_dataGridView.AutoSizeRowsMode            = DataGridViewAutoSizeRowsMode.AllCells;
            m_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            Controls.Add(m_dataGridView);
            WindowState = FormWindowState.Maximized;
            // Size = new Size(1000, 500);
        }

        protected abstract string ToolTipText(DataGridViewCellEventArgs e);

        protected void ToolTip(DataGridViewCellEventArgs e, bool showTip)
        {
            DataGridViewCell cell =
                RowIsHeader(e.RowIndex)
                ? m_dataGridView.Columns[e.ColumnIndex].HeaderCell
                : m_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            cell.ToolTipText = showTip ? ToolTipText(e) : String.Empty;
        }

        protected void CellMouseEnter(object sender, DataGridViewCellEventArgs e)
           => ToolTip(e, true);

        protected void CellMouseLeave(object sender, DataGridViewCellEventArgs e)
            => ToolTip(e, false);

        protected bool RowIsHeader(int iRow)
            => iRow < 0;

        protected bool RowIsFooter(int iRow)
            => iRow >= m_dataGridView.RowCount;

        protected bool ColIsKeyArea(int iCol)
            => iCol < VerticalAxis.NrOfLeadingElements;

        protected bool ColIsSummary(int iCol)
            => iCol == m_dataGridView.ColumnCount - 1;

        protected virtual string FormatMatrixElement(int iValue)
            => (iValue > 0) ? $"{ iValue, -3 }" : "   ";

        private void m_labelFormName_Click(object sender, EventArgs e)
        {

        }

        private void m_labelTimeRange_Click(object sender, EventArgs e)
        {

        }

        private void m_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

