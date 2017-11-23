namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class FileProxyDelayTimes
    {
        public int GetAsyncFullTableDelay { get; set; } = 10000;
        public int SingleEntryDelay { get; set; } = 2000;
    }
}
