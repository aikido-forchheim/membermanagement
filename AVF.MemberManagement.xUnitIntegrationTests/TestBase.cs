using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class TestBase
    {
        public const string AppId = "10bc9068-17ac-4f0f-a596-7fdfe20bc9f4";

        public Bootstrapper Bootstrapper { get; }

        public TestBase()
        {
            Bootstrapper = new Bootstrapper(false);
            Bootstrapper.Run();

            Bootstrapper.Container.RegisterType<ITokenService, UnitTestTokenService>();
        }
    }
}
