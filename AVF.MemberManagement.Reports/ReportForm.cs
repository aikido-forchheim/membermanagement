using System;
using System.Drawing;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class ReportForm : Form
    {
        private DatabaseWrapper  m_dbWrapper;
        private DateTime         m_datStart;
        private DateTime         m_datEnd;
        private ReportBase       m_Report;
        private ReportDescriptor m_reportDescriptor;

        public ReportForm(DatabaseWrapper db, ReportDescriptor desc )
        {
            InitializeComponent( );
            Load += new EventHandler(ReportFormLoad);
            m_dbWrapper = db;
            m_reportDescriptor = desc;
        }

        private void ReportFormLoad(System.Object sender, EventArgs e)
        {
            int iJahr = 2017;

            m_datStart = new DateTime(iJahr, 1, 1);
            m_datEnd = new DateTime(iJahr, 12, 31);

            Controls.Add(m_dataGridView);

            Size = new Size(1000, 500);

            m_Report.ShowReport(m_reportDescriptor);
        }
    }
}
