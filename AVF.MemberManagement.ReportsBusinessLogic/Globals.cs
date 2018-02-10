using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class Globals
    {
        public static DatabaseWrapper DatabaseWrapper;

        public static void Initialize(IUnityContainer Container)
        {
            DatabaseWrapper = new DatabaseWrapper();
            DatabaseWrapper.ReadTables(Container).Wait();
        }
    }
}
