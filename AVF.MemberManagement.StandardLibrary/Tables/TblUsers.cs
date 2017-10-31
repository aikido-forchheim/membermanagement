using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{
    public class TblUsers : ITable<User>
    {
        public const string TableName = "Users";

        public string Uri { get; set; } = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<User> Rows
        {
            get;
            set;
        }
    }
}
