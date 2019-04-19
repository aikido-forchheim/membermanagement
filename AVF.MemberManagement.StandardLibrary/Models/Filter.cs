namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class Filter
    {
        public string ColumnName { get; set; }
        public string MatchType { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"filter={ColumnName ?? string.Empty},{MatchType ?? string.Empty},{Value ?? string.Empty}";
        }
    }
}