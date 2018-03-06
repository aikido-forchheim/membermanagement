using System;
using System.Drawing;
using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public abstract class ReportBase : Panel
    {
        // properties

        protected DataGridView  P_dataGridView { get; set; }
        protected Label         P_labelReportName { get; set; }
        protected Font          P_font { get; set; }

        private Color P_ColorCell { get; set; }

        private const int BORDER_TOP = 200;

        // constructor

        public ReportBase()
            => Resize += new EventHandler
               (
                   delegate(object sender, System.EventArgs e)
                   {
                       int extraHeight = 2;   // values found by trial and error, to prevend scroll bars
                       int extraWidth = 20;   // TODO: find clean solution
                       int maxWidth = P_dataGridView.Columns.GetColumnsWidth(DataGridViewElementStates.None) + extraWidth;
                       int maxHeight = P_dataGridView.Rows.GetRowsHeight(DataGridViewElementStates.None) + extraHeight + P_dataGridView.ColumnHeadersHeight;
                       P_dataGridView.Width  = Math.Min(ClientSize.Width  - P_dataGridView.Location.X, maxWidth);
                       P_dataGridView.Height = Math.Min(ClientSize.Height - P_dataGridView.Location.Y, maxHeight);
                   }
               );

        // abstract memeber functions

        protected abstract void   ReportFormPopulate();    // Fill cells of DataGridView
        protected abstract string MouseCellEvent(int row, int col, bool action);

        // member functions

        protected virtual void InitializeReportBase()
        {
            P_font = new Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            P_labelReportName = new Label();
            P_dataGridView = new DataGridView();

            // 
            // P_labelReportName
            // 
            P_labelReportName.AutoSize = true;
            P_labelReportName.Font = P_font;
            P_labelReportName.Location = new System.Drawing.Point(0, 30);
            P_labelReportName.Name = "P_labelReportName";
            P_labelReportName.TabIndex = 1;

            // 
            // P_dataGridView
            // 
            ((System.ComponentModel.ISupportInitialize)(P_dataGridView)).BeginInit();
            P_dataGridView.AllowUserToAddRows = false;
            P_dataGridView.AllowUserToDeleteRows = false;
            P_dataGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            P_dataGridView.EnableHeadersVisualStyles = false;
            P_dataGridView.Location = new Point(0, BORDER_TOP);
            P_dataGridView.MultiSelect = true;
            P_dataGridView.Name = "P_dataGridView";
            P_dataGridView.RowHeadersVisible = false;
            P_dataGridView.RowHeadersWidth = 20;
            P_dataGridView.RowTemplate.Height = 28;
            P_dataGridView.Size = new Size(1345, 712);
            P_dataGridView.TabIndex = 0;
            P_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            P_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            P_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ((System.ComponentModel.ISupportInitialize)(P_dataGridView)).EndInit();

            P_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(delegate(Object sender, DataGridViewCellMouseEventArgs e) { MouseCellEvent(e.RowIndex, e.ColumnIndex, action: true); } );
            P_dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(delegate (object sender, DataGridViewCellEventArgs e) { ToolTip(e, true); });
            P_dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(delegate (object sender, DataGridViewCellEventArgs e) { ToolTip(e, false); });

            Controls.Add(P_dataGridView);
            Controls.Add(P_labelReportName);

            Dock = DockStyle.Fill;
            BackColor = Color.AliceBlue;
        }

        protected void ToolTip(DataGridViewCellEventArgs e, bool showTip)
        {
            DataGridViewCell cell =
                RowIsHeader(e.RowIndex)
                ? P_dataGridView.Columns[e.ColumnIndex].HeaderCell
                : P_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (showTip)
            {
                cell.ToolTipText = MouseCellEvent(e.RowIndex, e.ColumnIndex, action: false);
                if ( cell.ToolTipText != String.Empty )
                {
                    P_ColorCell = cell.Style.BackColor;
                    cell.Style.BackColor = Color.LawnGreen;
                }
            }
            else
            {
                cell.Style.BackColor = P_ColorCell;
                cell.ToolTipText = String.Empty;
            }
        }

        protected bool RowIsHeader(int iRow)
           => iRow < 0;
    }
}
