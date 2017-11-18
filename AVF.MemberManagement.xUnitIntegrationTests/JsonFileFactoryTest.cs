using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class JsonFileFactoryTest : TestBase
    {
        [Fact]
        public async void RefreshFileCacheTest()
        {
            var factory = Bootstrapper.Container.Resolve<IJsonFileFactory>();
            var fileList = await factory.RefreshFileCache();
            
            Assert.True(fileList.Count > 0);
        }
    }
}