using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class User : UserBase, IIntId
    {
        public const string PrimaryKey = "UserID";

        [JsonIgnore]
        public string PrimaryKeyName { get; set; } = PrimaryKey;

	    [JsonProperty(PropertyName = PrimaryKey)]
        public int Id
		{
			get;
			set;
		}
	}
}
