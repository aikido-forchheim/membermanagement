using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Services
{
    public class Globals
    {
        public static bool UsesXamarinAuth = true;

        public static IAccountService AccountService;

        public static User User { get; set; }

        public static bool UseFileProxies { get; set; }
    }
}
