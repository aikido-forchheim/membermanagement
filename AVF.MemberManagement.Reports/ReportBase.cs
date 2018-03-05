﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class ReportBase : Panel
    {
        protected DataGridView   m_dataGridView;
        protected Label          m_labelReportName;
        protected Font           m_font;
        protected AxisTypeMember m_axisTypeMember { get; private set; }

        private Color m_ColorCell;

        int BORDER_TOP = 200;

        public ReportBase()
            => Resize += new EventHandler(ReportResize);

        protected virtual void InitializeReportBase()
        {
            m_font            = new Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            m_labelReportName = new Label();
            m_dataGridView    = new DataGridView();
            m_axisTypeMember  = new AxisTypeMember();

            // 
            // m_labelReportName
            // 
            m_labelReportName.AutoSize = true;
            m_labelReportName.Font = m_font;
            m_labelReportName.Location = new System.Drawing.Point(0, 30);
            m_labelReportName.Name = "m_labelReportName";
            m_labelReportName.TabIndex = 1;

            // 
            // m_dataGridView
            // 
            ((System.ComponentModel.ISupportInitialize)(m_dataGridView)).BeginInit();
            m_dataGridView.AllowUserToAddRows = false;
            m_dataGridView.AllowUserToDeleteRows = false;
            m_dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            m_dataGridView.EnableHeadersVisualStyles = false;
            m_dataGridView.Location = new Point(0, BORDER_TOP);
            m_dataGridView.MultiSelect = true;
            m_dataGridView.Name = "m_dataGridView";
            m_dataGridView.RowHeadersVisible = false;
            m_dataGridView.RowHeadersWidth = 20;
            m_dataGridView.RowTemplate.Height = 28;
            m_dataGridView.Size = new Size(1345, 712);
            m_dataGridView.TabIndex = 0;
            m_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            m_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            m_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ((System.ComponentModel.ISupportInitialize)(m_dataGridView)).EndInit();

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
            m_dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            m_dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);

            Controls.Add(m_dataGridView);
            Controls.Add(m_labelReportName);

            Dock = DockStyle.Fill;
            BackColor = Color.AliceBlue;
        }

        private void ReportResize(object sender, System.EventArgs e)
        {
            int extraHeight = 2;   // values found by trial and error, to prevend scroll bars
            int extraWidth  = 20;  // TODO: find clean solution
            int maxWidth    = m_dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.None) + extraWidth;
            int maxHeight   = m_dataGridView.Rows.   GetRowsHeight  (DataGridViewElementStates.None) + extraHeight + m_dataGridView.ColumnHeadersHeight;
            m_dataGridView.Width  = Math.Min(ClientSize.Width  - m_dataGridView.Location.X,   maxWidth);
            m_dataGridView.Height = Math.Min(ClientSize.Height - m_dataGridView.Location.Y, maxHeight);
        }

        protected void ToolTip(DataGridViewCellEventArgs e, bool showTip)
        {
            DataGridViewCell cell =
                RowIsHeader(e.RowIndex)
                ? m_dataGridView.Columns[e.ColumnIndex].HeaderCell
                : m_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (showTip)
            {
                cell.ToolTipText = MouseCellEvent(e.RowIndex, e.ColumnIndex, action: false);
                if ( cell.ToolTipText != String.Empty )
                {
                    m_ColorCell = cell.Style.BackColor;
                    cell.Style.BackColor = Color.LawnGreen;
                }
            }
            else
            {
                cell.Style.BackColor = m_ColorCell;
                cell.ToolTipText = String.Empty;
            }
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
