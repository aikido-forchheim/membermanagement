using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class Test : IIntId
    {
        public const string PrimaryKey = "ID";

        [JsonProperty(PropertyName = PrimaryKey)]
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
