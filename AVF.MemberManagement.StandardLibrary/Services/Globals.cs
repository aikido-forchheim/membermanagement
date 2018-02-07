using System;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Enums;
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

        public static Idiom Idiom { get; set; }

        
        public static async Task<Wochentag> GetWochentagFromDayOfWeek(IRepository<Wochentag> wochentageRepository, DayOfWeek dayOfWeek)
        {
            var weekday = (int)dayOfWeek;
            if (weekday == 0) weekday = 7;
            var wochentage = await wochentageRepository.GetAsync();
            var wochentag = wochentage.Single(wd => wd.Id == weekday);
            return wochentag;
        }
    }
}
