using AVF.MemberManagement.StandardLibrary.Interfaces;

namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class FileProxyDelayTimes : IFileProxyDelayTimes
    {
        public int GetAsyncFullTableDelay { get; set; } = 10000;
        public int GetAsyncSingleEntryDelay { get; set; } = 2000;


        public int GetFullTableDelayGetAsync()
        {
            return GetAsyncFullTableDelay;
        }

        public int GetSingleEntryDelayGetAsync()
        {
            return GetAsyncFullTableDelay;
        }
    }
}
