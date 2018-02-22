using System;
using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class ReportBase : Form
    {
        protected DataGridView m_dataGridView;

        public ReportBase()
        {
            Load += new EventHandler(ReportFormLoad);
        }

        private void ReportFormLoad(System.Object sender, EventArgs e)
        {
            ReportFormPopulate();

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
            m_dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            m_dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);

            m_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            m_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            m_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            Controls.Add(m_dataGridView);
            WindowState = FormWindowState.Maximized;
        }

        protected void ToolTip(DataGridViewCellEventArgs e, bool showTip)
        {
            DataGridViewCell cell =
                RowIsHeader(e.RowIndex)
                ? m_dataGridView.Columns[e.ColumnIndex].HeaderCell
                : m_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            cell.ToolTipText = showTip ? MouseCellEvent(e.RowIndex, e.ColumnIndex, action: false) : String.Empty;
        }

        protected virtual string MouseCellEvent(int row, int col, bool action)
            => String.Empty;

        protected void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
            => MouseCellEvent(e.RowIndex, e.ColumnIndex, action: true);

        private void CellMouseEnter(object sender, DataGridViewCellEventArgs e)
           => ToolTip(e, true);

        private void CellMouseLeave(object sender, DataGridViewCellEventArgs e)
            => ToolTip(e, false);

        protected bool RowIsHeader(int iRow)
           => iRow < 0;

        protected abstract void ReportFormPopulate();    // Fill cells of DataGridView

    }
}
