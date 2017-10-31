using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Models.Tbo
{
    public class Test : IIntId
    {
        [JsonProperty(PropertyName = "ID")]
        public int Id
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}
