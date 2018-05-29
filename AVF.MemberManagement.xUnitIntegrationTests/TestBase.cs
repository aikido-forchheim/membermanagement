using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Practices.Unity;
using Prism.Ioc;
using Xamarin.Forms;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class TestBase
    {
        public const string AppId = "10bc9068-17ac-4f0f-a596-7fdfe20bc9f4";

        public Bootstrapper Bootstrapper { get; }

        public TestBase()
        {
            Forms.Init();

            Bootstrapper = new Bootstrapper(false);

            Bootstrapper.ContainerRegistry.Register<ITokenService, UnitTestTokenService>();
        }
    }
}
