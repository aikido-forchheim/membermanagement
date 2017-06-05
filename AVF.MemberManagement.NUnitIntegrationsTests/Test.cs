using NUnit.Framework;
using System;
namespace AVF.MemberManagement.NUnitIntegrationsTests
{
    [TestFixture()]
    public class Test
    {
        [Test()]
        public void TestCase()
        {
            var x = xUnitIntegrationTests.IntegrationTestSettings.Get();

            Assert.IsNotNull(x);
        }
    }
}
