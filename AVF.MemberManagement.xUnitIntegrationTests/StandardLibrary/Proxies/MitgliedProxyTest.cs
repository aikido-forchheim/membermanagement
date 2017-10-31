using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Proxies
{
    public class MitgliedProxyTest : TestBase
    {
        [Fact]
        public void FilterTest()
        {
            var mitgliederProxy = Bootstrapper.Container.Resolve<IProxy<Mitglied>>();

            mitgliederProxy.
        }
    }
}
