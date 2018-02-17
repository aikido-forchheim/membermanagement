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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var bootstrapper = new Bootstrapper(false);

            bootstrapper.Run();

            m_container = bootstrapper.Container;
            System.Console.WriteLine("read database");
            Globals.Initialize(m_container);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int iJahr = 2017;

            Application.Run(new ReportMemberVsCourses(new DateTime(iJahr, 1, 1), new DateTime(iJahr, 12, 31)));
        }
    }
}
