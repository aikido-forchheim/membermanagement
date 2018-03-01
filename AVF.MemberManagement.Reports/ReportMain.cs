using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using AVF.MemberManagement.xUnitIntegrationTests;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class ReportMain : Form
    {
        private static IUnityContainer m_container;

        public ReportMain()
        {
            InitializeComponent();
            InitDatabase();
        }

        private async Task InitDatabase()
        {
            var bootstrapper = new Bootstrapper(false);
            bootstrapper.Run();
            m_container = bootstrapper.Container;
            await Globals.Initialize
            (
                m_container, 
                tick: s => { progressBar1.PerformStep(); labelAnimateLoadDb.Text = s;  }
            );
            Kurse.Enabled = true;
            MemberFees.Enabled = true;
            Graduierungsliste.Enabled = true;
            TrainingsteilnahmeKurse.Enabled = true;
            TrainingsteilnahmeMonate.Enabled = true;
            progressBar1.Hide();
            labelLadeDatenbank.Hide();
        }

        private void Trainingsteilnahme_Kurse_Click(object sender, EventArgs e)
        {
            int iJahr = 2017;
            var form = new ReportMemberVsCourses(new DateTime(iJahr, 1, 1), new DateTime(iJahr, 12, 31));
            form.ShowDialog();
        }

        private void Kurse_Click(object sender, EventArgs e)
        {
            int iJahr = 2017;
            var form = new ReportCoursesVsMonths(new DateTime(iJahr, 1, 1), new DateTime(iJahr, 12, 31), -1);
            form.ShowDialog();
        }

        private void Trainingsteilnahme_Monate_Click(object sender, EventArgs e)
        {
            int iJahr = 2017;
            var form = new ReportMemberVsMonths(new DateTime(iJahr, 1, 1), new DateTime(iJahr, 12, 31));
            form.ShowDialog();
        }

        private void Gradierungsliste_Click(object sender, EventArgs e)
        {
            var form = new ReportGraduationList();
            form.ShowDialog();
        }

        private void MemberFees_Click(object sender, EventArgs e)
        {
            var form = new ReportMemberFees();
            form.ShowDialog();
        }

        private void ApplicationExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
