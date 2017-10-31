using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Models.Tbo
{
    public class User : UserBase
	{
	    [JsonProperty(PropertyName = "UserId")]
        public int Id
		{
			get;
			set;
		}
	}
}
