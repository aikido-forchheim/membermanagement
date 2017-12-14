using AVF.MemberManagement.StandardLibrary.Interfaces;

namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class FileProxyDelayTimes : IFileProxyDelayTimes
    {
        public int GetAsyncFullTableDelay { get; set; }
        public int GetAsyncSingleEntryDelay { get; set; }


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