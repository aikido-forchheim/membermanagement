using System;
using System.Collections.Generic;
using System.Text;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class TestObject : IId<string>
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

        public string VarChar50
        {
            get;
            set;
        }

        public string Json
        {
            get;
            set;
        }

        public int Int
        {
            get;
            set;
        }
    }
}
