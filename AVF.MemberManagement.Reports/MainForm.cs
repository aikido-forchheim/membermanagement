using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class MainForm : Form
    {
        private DatabaseWrapper  m_dbWrapper;
        private DataGridView     m_dataGridView;
        private ReportBase       m_report;
        private DateTime         m_datStart;
        private DateTime         m_datEnd;
        private Stack            m_ReportStack = new Stack();
        private ReportDescriptor m_CurrentReport;

        public MainForm(DatabaseWrapper db)
        {
            InitializeComponent();
            Load += new EventHandler(MainFormLoad);
            m_dbWrapper = db;
        }

        private class ReportDescription
        {
            public ReportDescription(int? idMember, int? idCourse, int? idWeek)
            {
                m_idMember = idMember;
                m_idCourse = idCourse;
                m_idWeek   = idWeek;
            }

            public int? m_idMember { get; set; }
            public int? m_idCourse { get; set; }
            public int? m_idWeek   { get; set; }
        }

        private enum ReportType { MemberVsTraining, MemberVsCourse, WeekVsCourse };

        private class ReportDescriptor
        {
            public ReportDescriptor(ReportType type, int param = 0)
            {
                m_type = type;
                m_param = param;
            }

            public ReportType m_type  { get; set; }
            public int        m_param { get; set; }
        }

        private void ShowReport( ReportDescriptor desc )
        {
            switch ( desc.m_type )
            {
                case ReportType.MemberVsTraining:
                    m_report = new ReportMemberVsTraining(m_dbWrapper, m_datStart, m_datEnd, desc.m_param);
                    m_report.PopulateGridView(m_dataGridView);
                    break;
                case ReportType.MemberVsCourse:
                    m_report = new ReportMemberVsCourse(m_dbWrapper, m_datStart, m_datEnd);
                    m_report.PopulateGridView(m_dataGridView);
                    break;
                case ReportType.WeekVsCourse:
                    m_report = new ReportWeekVsCourse(m_dbWrapper, m_datStart, m_datEnd, desc.m_param);
                    m_report.PopulateGridView(m_dataGridView);
                    break;
            }
        }

        private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            m_ReportStack.Push( m_CurrentReport );

            if ((e.RowIndex == -1) || (e.RowIndex == m_dataGridView.RowCount - 1))
            {
                if ((3 <= e.ColumnIndex) && (e.ColumnIndex <= 13))
                {
                    int idKurs = e.ColumnIndex - m_report.m_iNrOfColsOnLeftSide;
                    m_CurrentReport = new ReportDescriptor(ReportType.MemberVsTraining, idKurs);
                }
            }
            else
            {
                if ((e.ColumnIndex < 3) || (e.ColumnIndex == 14))
                {
                    int idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                    m_CurrentReport = new ReportDescriptor(ReportType.WeekVsCourse, idMember);
                }
                else  // main area
                {
                    int idKurs = (e.ColumnIndex == 3) ? 0 : e.ColumnIndex - m_report.m_iNrOfColsOnLeftSide;
                    m_CurrentReport = new ReportDescriptor(ReportType.MemberVsTraining, idKurs);
                }
            }

            ShowReport( m_CurrentReport );
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
            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);

            Size = new Size(1000, 500);

            m_CurrentReport = new ReportDescriptor(ReportType.MemberVsCourse);
            ShowReport( m_CurrentReport );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (m_ReportStack.Count > 0)
            {
                m_CurrentReport = (ReportDescriptor)m_ReportStack.Pop();
                ShowReport( m_CurrentReport );
            }
        }
    }
}