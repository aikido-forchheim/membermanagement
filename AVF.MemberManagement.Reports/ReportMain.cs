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

        private int m_iYear;
        public  int P_iJahr
        {
            set{
                   if ( (Globals.DatabaseWrapper.GetStartValidData().Year <= value) && (value <= DateTime.Now.Year) )
                       m_iYear = value;
               }
            get{ return m_iYear; }
        }

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
            P_iJahr = DateTime.Now.Year - 1;
            m_UndoRedo = new ReportsUndoRedo(P_formMain);
            m_UndoRedo.SwitchTo(new ReportMemberVsCourses(new DateTime(P_iJahr, 1, 1), new DateTime(P_iJahr, 12, 31)));
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
            => SwitchToPanel(new ReportMemberVsCourses(new DateTime(P_iJahr, 1, 1), new DateTime(P_iJahr, 12, 31)));

        private void Kurse_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportCoursesVsMonths(new DateTime(P_iJahr, 1, 1), new DateTime(P_iJahr, 12, 31), -1));

        private void Trainingsteilnahme_Monate_Click(object sender, EventArgs e)
             => SwitchToPanel(new ReportMemberVsMonths(new DateTime(P_iJahr, 1, 1), new DateTime(P_iJahr, 12, 31), -1 ));

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
