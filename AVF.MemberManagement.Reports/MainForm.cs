using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using AVF.MemberManagement.ReportBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class MainForm : Form
    {
        private DatabaseWrapper m_dbWrapper;

        public MainForm(DatabaseWrapper db)
        {
            InitializeComponent();
            Load += new EventHandler(MainFormLoad);
            m_dbWrapper = db;
        }

        private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            StringBuilder messageBoxCS = new StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "ColumnIndex", e.ColumnIndex);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "RowIndex", e.RowIndex);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Button", e.Button);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Clicks", e.Clicks);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "X", e.X);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Y", e.Y);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Delta", e.Delta);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Location", e.Location);
            messageBoxCS.AppendLine();
            MessageBox.Show(messageBoxCS.ToString(), "CellMouseClick Event");
        }

        private void MainFormLoad(System.Object sender, EventArgs e)
        {
            int iJahr = 2017;

            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);

            ReportMemberVsCourse tp = new ReportMemberVsCourse(m_dbWrapper, datStart, datEnd);
            DataGridView dataGridView = tp.GetMatrix();

            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dataGridView.Location = new Point(8, 8);
            dataGridView.Size = new Size(500, 250);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView.GridColor = Color.Black;
            dataGridView.RowHeadersVisible = false;

            dataGridView.SelectionMode =  DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.Dock = DockStyle.Fill;

            dataGridView.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.Columns[0].HeaderText = "ABC\nDEF";
            dataGridView.AutoResizeColumnHeadersHeight();
            dataGridView.AutoResizeColumns();

            Controls.Add(dataGridView);
            dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);

            Size = new Size(600, 500);
        }

    }
}