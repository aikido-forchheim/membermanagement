using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using AVF.MemberManagement.xUnitIntegrationTests;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.Reports.Properties;

namespace AVF.MemberManagement.Reports
{
    public partial class ReportMain : Form
    {
        private UndoRedoStack m_UndoRedo;

        public static ReportMain P_formMain { set; get; }

        private int P_iJahr = 2017;

        public ReportMain()
        {
            P_formMain = this;
            InitializeComponent();
            InitDatabase();
        }

        private async Task InitDatabase()
        {
            IUnityContainer P_container;
            var bootstrapper = new Bootstrapper(false);
            bootstrapper.Run();
            P_container = bootstrapper.Container;
            await Globals.Initialize
            (
                P_container, 
                tick: s => { progressBar1.PerformStep(); labelAnimateLoadDb.Text = s; }
            );
            panelLoadDb.Dispose();
            m_UndoRedo = new UndoRedoStack(P_formMain, buttonUndo, buttonRedo, new ReportMemberVsCourses(new DateTime(P_iJahr, 1, 1), new DateTime(P_iJahr, 12, 31)));
        }

        public string SwitchToPanel(ReportBase panelNew )
        {
            m_UndoRedo.Add(panelNew);
            return String.Empty;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (Settings.Default.WindowLocation != null)
                Location = Settings.Default.WindowLocation;

            if (Settings.Default.WindowSize != null)
                Size = Settings.Default.WindowSize;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.WindowLocation = this.Location;   // Copy window location to app settings
            
            Settings.Default.WindowSize =                     // Copy window size to app settings
                (this.WindowState == FormWindowState.Normal) 
                ? this.Size 
                : this.RestoreBounds.Size;
            
            Settings.Default.Save();         // Save settings
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
            => Application.Exit();

        private void Export_Click(object sender, EventArgs e)
            => m_UndoRedo.P_reportActual.Export2Excel();

        private void Undo_Click(object sender, EventArgs e)
            => m_UndoRedo.Undo();

        private void Redo_Click(object sender, EventArgs e)
            => m_UndoRedo.Redo();
    }
}
