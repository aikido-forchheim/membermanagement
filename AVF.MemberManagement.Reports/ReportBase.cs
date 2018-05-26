using System;
using System.Drawing;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class ReportBase : Panel
    {
        // properties

        protected DataGridView P_dataGridView { get; set; }
        protected Label        P_labelReportName { get; set; }
        protected Font         P_font { get; set; }

        private Color P_ColorCell { get; set; }

        private const int BORDER_TOP = 200;

        // constructor

        public ReportBase()
            => Resize += new EventHandler( delegate(object s, System.EventArgs e) { SizeDataGridView(P_dataGridView); } );

        // abstract memeber functions

        protected abstract void   ReportFormPopulate(Action<String> tick);    // Fill cells of DataGridView
        protected abstract string MouseCellEvent(int row, int col, MouseButtons buttons, bool action);

        // member functions

        public void SizeDataGridView( DataGridView dgv )
        {
            int extraHeight = 2;   // values found by trial and error, to prevend scroll bars
            int extraWidth = 20;   // TODO: find clean solution
            int maxWidth = dgv.Columns.GetColumnsWidth(DataGridViewElementStates.None) + extraWidth;
            int maxHeight = dgv.Rows.GetRowsHeight(DataGridViewElementStates.None) + extraHeight + dgv.ColumnHeadersHeight;
            dgv.Width  = Math.Min(ClientSize.Width  - dgv.Location.X, maxWidth);
            dgv.Height = Math.Min(ClientSize.Height - dgv.Location.Y, maxHeight);
        }

        public void Export2Excel()
            => ExcelExport.Export2Excel( P_dataGridView, 2, 1, GetType().Name );

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
            P_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler
            (
                delegate (object sender, DataGridViewCellMouseEventArgs e) 
                {
                    MouseCellEvent(e.RowIndex, e.ColumnIndex, e.Button, action: true);
                }
            );

            ((System.ComponentModel.ISupportInitialize)(P_dataGridView)).EndInit();
            Controls.Add(P_dataGridView);
            Controls.Add(P_labelReportName);

            Dock = DockStyle.Fill;
            BackColor = Color.AliceBlue;
        }

        protected bool IsHeaderRow(int iRow)
           => iRow < 0;

        protected bool IsSummaryRow(int iRow)
           => iRow == P_dataGridView.RowCount - 1;
    }
}
