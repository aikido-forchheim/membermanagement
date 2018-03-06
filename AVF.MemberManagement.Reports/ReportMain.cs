using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using AVF.MemberManagement.xUnitIntegrationTests;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.Reports.Properties;

namespace AVF.MemberManagement.Reports
{
    public partial class ReportMain : Form
    {
        private static IUnityContainer P_container;

        private static Panel P_panelActual;
        private static Form P_formMain;

        private int P_iJahr = 2017;

        public ReportMain()
        {
            P_formMain = this;
            InitializeComponent();
            InitDatabase();
        }

        private async Task InitDatabase()
        {
            var bootstrapper = new Bootstrapper(false);
            bootstrapper.Run();
            P_container = bootstrapper.Container;
            panelButtons.BringToFront();
            await Globals.Initialize
            (
                P_container, 
                tick: s => { progressBar1.PerformStep(); labelAnimateLoadDb.Text = s;  }
            );
            foreach (Control control in panelButtons.Controls)
                control.Enabled = true;
            panelLoadDb.Dispose();
            P_panelActual = panelButtons;
        }

        public static string SwitchToPanel( Panel panelNew )
        {
            P_panelActual.Dispose();
            P_panelActual = panelNew;
            P_formMain.Controls.Add(P_panelActual);
            return String.Empty;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // Set window location
            if (Settings.Default.WindowLocation != null)
            {
                Location = Settings.Default.WindowLocation;
            }

            // Set window size
            if (Settings.Default.WindowSize != null)
            {
                Size = Settings.Default.WindowSize;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Copy window location to app settings
            Settings.Default.WindowLocation = this.Location;

            // Copy window size to app settings
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowSize = this.Size;
            }
            else
            {
                Settings.Default.WindowSize = this.RestoreBounds.Size;
            }

            // Save settings
            Settings.Default.Save();
        }

        private void Trainingsteilnahme_Kurse_Click(object sender, EventArgs e)
            => SwitchToPanel(new ReportMemberVsCourses(new DateTime(P_iJahr, 1, 1), new DateTime(P_iJahr, 12, 31)));

        private void Kurse_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportCoursesVsMonths(new DateTime(P_iJahr, 1, 1), new DateTime(P_iJahr, 12, 31), -1));

        private void Trainingsteilnahme_Monate_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportMemberVsMonths(new DateTime(P_iJahr, 1, 1), new DateTime(P_iJahr, 12, 31)));

        private void Gradierungsliste_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportGraduationList());

        private void MemberFees_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportMemberFees());

        private void ApplicationExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void labelAnimateLoadDb_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
