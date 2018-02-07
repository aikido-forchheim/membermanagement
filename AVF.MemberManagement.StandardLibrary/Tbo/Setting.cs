using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class Setting : IId<string>
    {
        public const string PrimaryKey = "Key";

        [JsonIgnore]
        public string PrimaryKeyName { get; set; } = PrimaryKey;

	    [JsonProperty(PropertyName = PrimaryKey)]
        public string Id
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public string Des
		{
			get;
			set;
		}

        public override string ToString()
        {
            return string.Format("[Setting: Id={0}, Value={1}, Des={2}]", Id, Value, Des);
        }
	}
}
