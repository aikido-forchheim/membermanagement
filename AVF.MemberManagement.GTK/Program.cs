using System;
using System.Net;
using AVF.MemberManagement.GTK.Services;
using AVF.MemberManagement.StandardLibrary.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace AVF.MemberManagement.GTK
{
    public class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Globals.UsesXamarinAuth = false;
            Globals.AccountService = new AccountServiceS();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            Gtk.Application.Init();
            Forms.Init();
            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("AVF.MemberManagement.GTK");
            //window.Icon = Pixbuf.LoadFromResource("XamGtkExplorer.GTK.Application-icon.png");
            window.Show();
            Gtk.Application.Run();
        }
    }
}
