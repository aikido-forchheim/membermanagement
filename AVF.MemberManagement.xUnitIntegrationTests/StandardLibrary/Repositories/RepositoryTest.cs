using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using System.Linq;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Repositories
{
    public class RepositoryTest : TestBase
    {
        [Fact]
        public async Task CreateDeleteTest()
        {
            Test testObject = new Test();
            testObject.Text = "Test";

            Assert.True(testObject.Id == 0);

            var testRepository = Bootstrapper.Container.Resolve<IRepository<Test>>();

            var createResult = await testRepository.CreateAsync(testObject);

            Assert.True(createResult == 0);
            Assert.True(testObject.Id != 0);

            var list1 = await testRepository.GetAsync();
            var maxId = list1.Select(l => l.Id).Max();
            Assert.True(maxId == testObject.Id);

            var deleteResult = await testRepository.DeleteAsync(testObject);

            Assert.True(deleteResult == 1);

            var list2 = await testRepository.GetAsync();
            maxId = list2.Select(l => l.Id).Max();
            Assert.True(maxId < testObject.Id);
        }
    }
}
