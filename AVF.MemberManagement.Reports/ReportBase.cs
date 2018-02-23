using System;
using System.Drawing;
using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class ReportBase : Form
    {
        protected DataGridView m_dataGridView;

        public ReportBase()
        { 
            Load += new EventHandler(ReportFormLoad);
            Resize += new EventHandler(ReportResize);
        }

        protected void ReportFormLoad(System.Object sender, EventArgs e)
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

        private void ReportResize(object sender, System.EventArgs e)
        {
            Control control = (Control)sender;

            int newWidth = m_dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.None) + 20;
            int maxHeight = m_dataGridView.Rows.GetRowsHeight(DataGridViewElementStates.None) + m_dataGridView.ColumnHeadersHeight + 2;
            int newHeight = Math.Min( ClientSize.Height - m_dataGridView.Location.Y - 10, maxHeight);
            m_dataGridView.Size = new System.Drawing.Size(newWidth, newHeight);
        }

        protected void ToolTip(DataGridViewCellEventArgs e, bool showTip)
        {
            DataGridViewCell cell =
                RowIsHeader(e.RowIndex)
                ? m_dataGridView.Columns[e.ColumnIndex].HeaderCell
                : m_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            cell.ToolTipText = showTip ? MouseCellEvent(e.RowIndex, e.ColumnIndex, action: false) : String.Empty;
        }

        protected abstract string MouseCellEvent(int row, int col, bool action);

        private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
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
