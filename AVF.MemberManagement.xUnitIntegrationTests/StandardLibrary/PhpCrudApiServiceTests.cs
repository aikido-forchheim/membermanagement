using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Xunit;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class PhpCrudApiServiceTests : TestBase
    {
        [Fact]
        public void GetObjectWithoutWrapper()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            //phpCrudApiService.
        }
    }
}
