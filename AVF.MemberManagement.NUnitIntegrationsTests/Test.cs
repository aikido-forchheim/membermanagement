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
            var integrationTestSettings = xUnitIntegrationTests.IntegrationTestSettings.Get();

            Assert.IsNotNull(integrationTestSettings);
        }
    }
}
