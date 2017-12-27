using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AVF.MemberManagement.iOS.Services;
using Foundation;
using UIKit;

namespace AVF.MemberManagement.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            StandardLibrary.Services.Globals.UsesXamarinAuth = false;
            StandardLibrary.Services.Globals.AccountService = new AccountServiceS();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
