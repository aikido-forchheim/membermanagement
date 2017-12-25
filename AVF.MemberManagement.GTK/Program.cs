using System;
using System.Linq;
using System.Net;
using AVF.MemberManagement.GTK.Services;
using AVF.MemberManagement.StandardLibrary.Services;
using Gdk;
using Gtk;
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
        private static void Main(string[] args)
        {
            if (args != null)
            {
                if (args.Contains("/usefileproxies"))
                {
                    StandardLibrary.Services.Globals.UseFileProxies = true;
                }
            }

            StandardLibrary.Services.Globals.UsesXamarinAuth = false;
            StandardLibrary.Services.Globals.AccountService = new AccountServiceS();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            Gtk.Application.Init();
            Forms.Init();
            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("AVF.MemberManagement.GTK");
            window.Icon = Pixbuf.LoadFromResource("AVF.MemberManagement.GTK.AVF.ico");
            window.WidthRequest = 1024;
            window.HeightRequest = 600;
            window.WindowPosition = WindowPosition.Center;
            window.Show();
            Gtk.Application.Run();
        }
    }
}
