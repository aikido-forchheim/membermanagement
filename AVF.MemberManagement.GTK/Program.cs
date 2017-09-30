using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gdk;
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
        static void Main()
        {
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
