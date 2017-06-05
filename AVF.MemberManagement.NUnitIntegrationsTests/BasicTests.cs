using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AVF.MemberManagement.NUnitIntegrationsTests
{
    [TestFixture()]
    public class BasicTests : xUnitIntegrationTests.BasicTests
    {
        [Test()]
        public new void CouldIntegrationTestSettingsBeLoaded()
        {
            base.CouldIntegrationTestSettingsBeLoaded();
        }

        [Test()]
        public async new Task TestPhpCrudApiService()
        {
            await base.TestPhpCrudApiService();
        }

        [Test()]
		public async new Task VerfiyPassword()
		{
			await base.VerfiyPassword();
		}
    }
}
