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

        private TimeRange m_timeRange;

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
            m_UndoRedo = new ReportsUndoRedo(P_formMain);

            int iYear = DateTime.Now.Year - 1;
            m_timeRange = new TimeRange( new DateTime(iYear,  1,  1), new DateTime(iYear, 12, 31) );
            m_UndoRedo.SwitchTo(new ReportTrainingsParticipation(typeof(AxisTypeCourse), typeof(AxisTypeMember), m_timeRange, Globals.ALL_MEMBERS));
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
            Settings.Default.WindowLocation = Location;   // Copy window location to app settings
            
            Settings.Default.WindowSize =                     // Copy window size to app settings
                (WindowState == FormWindowState.Normal) 
                ? Size 
                : RestoreBounds.Size;
            
            Settings.Default.Save();         // Save settings
        }

        private void Trainingsteilnahme_Kurse_Click(object sender, EventArgs e)
            => SwitchToPanel(new ReportTrainingsParticipation(typeof(AxisTypeCourse), typeof(AxisTypeMember), m_timeRange, Globals.ALL_MEMBERS));

        private void Kurse_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportTrainingsParticipation(typeof(AxisTypeMonth), typeof(AxisTypeCourse), m_timeRange, Globals.ALL_MEMBERS));

        private void Trainingsteilnahme_Monate_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportTrainingsParticipation(typeof(AxisTypeMonth), typeof(AxisTypeMember), m_timeRange, Globals.ALL_MEMBERS, Globals.ALL_COURSES));

        private void Gradierungsliste_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportGraduationList());

        private void MemberFees_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportMemberFees());

        private void ApplicationExit_Click(object sender, EventArgs e)
            => Application.Exit();

        private void Export_Click(object sender, EventArgs e)
            => m_UndoRedo.P_reportActual.Export2Excel();

        private void Info_Click(object sender, EventArgs e)
        {
            string message = Globals.GetFullVersionInfo();
            string caption = "AVF Mitgliederdatenbank Reports";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message, caption, buttons);
        }

        private void Undo_Click(object sender, EventArgs e)
            => m_UndoRedo.Undo();

        private void Redo_Click(object sender, EventArgs e)
            => m_UndoRedo.Redo();

        public void UndoEnabled(bool state)
            => buttonUndo.Enabled = state;

        public void RedoEnabled(bool state)
            => buttonRedo.Enabled = state;
    }
}
