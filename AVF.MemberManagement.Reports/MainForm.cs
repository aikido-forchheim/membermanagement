using System;
using System.Drawing;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class MainForm : Form
    {
        private DatabaseWrapper m_dbWrapper;
        private DataGridView    m_dataGridView;
        private DateTime        m_datStart;
        private DateTime        m_datEnd;
        private ReportBase      m_Report;

        public MainForm(DatabaseWrapper db)
        {
            InitializeComponent();
            Load += new EventHandler(MainFormLoad);
            m_dbWrapper = db;
        }

        private void MainFormLoad(System.Object sender, EventArgs e)
        {
            int iJahr = 2017;

            m_datStart = new DateTime(iJahr, 1, 1);
            m_datEnd = new DateTime(iJahr, 12, 31);

            m_dataGridView = new DataGridView
            {
                MultiSelect = false,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };

            m_dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            m_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            m_dataGridView.Location = new Point(8, 80);
            m_dataGridView.Size = new Size(950, 500);
//            m_dataGridView.Dock = DockStyle.Fill;

            Controls.Add(m_dataGridView);

            Size = new Size(1000, 500);

            m_Report = new ReportMemberVsCourse(m_dbWrapper, m_datStart, m_datEnd);
            m_Report.ShowReport(new ReportDescriptor(ReportType.MemberVsCourse) );
        }
    }
}