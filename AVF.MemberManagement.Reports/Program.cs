using System;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using AVF.MemberManagement.xUnitIntegrationTests;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    static class Program
    {
        private static IUnityContainer m_container;
        private static DatabaseWrapper m_dbWrapper;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var bootstrapper = new Bootstrapper(false);

            bootstrapper.Run();

            m_container = bootstrapper.Container;
            m_dbWrapper = new DatabaseWrapper();
            System.Console.WriteLine("read database");
            m_dbWrapper.ReadTables(m_container).Wait();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ReportForm(m_dbWrapper, new ReportDescriptor(ReportType.MemberVsCourse)));
        }
    }
}
