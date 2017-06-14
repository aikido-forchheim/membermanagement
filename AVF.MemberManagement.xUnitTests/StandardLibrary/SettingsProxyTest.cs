using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AVF.MemberManagement.xUnitTests.StandardLibrary
{
    public class SettingsProxyTest
    {
        [Fact]
        public async void LoadSettingsCacheAsyncTest()
        {
            ISettingsProxy settingsProxy = new SettingsProxy(A.Fake<ILogger>(), A.Fake<IPhpCrudApiService>());
            var settingsCache = await settingsProxy.LoadSettingsCacheAsync();

            Assert.NotNull(settingsCache);
        }

        [Fact]
        public async void AssertNotExistingSettingReturnsNull()
        {
            ISettingsProxy settingsProxy = new SettingsProxy(A.Fake<ILogger>(), A.Fake<IPhpCrudApiService>());

            var notExistingSetting = await settingsProxy.GetSettingAsync("SomeNotExistingKey");

            Assert.Null(notExistingSetting);
        }

        [Fact]
        public async void AssertNotExistingSettingWithDefaultReturnsDefault()
        {
            ISettingsProxy settingsProxy = new SettingsProxy(A.Fake<ILogger>(), A.Fake<IPhpCrudApiService>());

            const string defaultValue = "IamDefault";
            var notExistingSetting = await settingsProxy.GetSettingAsync("SomeNotExistingKey", defaultValue);

            Assert.True(notExistingSetting.Value == defaultValue);
        }

    }
}
