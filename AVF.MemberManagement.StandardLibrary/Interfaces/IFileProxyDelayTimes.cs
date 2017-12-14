namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IFileProxyDelayTimes
    {
        int GetFullTableDelayGetAsync();
        int GetSingleEntryDelayGetAsync();
    }
}