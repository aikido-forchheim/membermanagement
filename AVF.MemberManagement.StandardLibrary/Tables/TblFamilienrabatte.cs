using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{
    public class TblFamilienrabatte : ITable<Familienrabatt>
    {
        public const string TableName = "tblFamilienrabatt";

        public string Uri { get; set; } = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<Familienrabatt> Rows { get; set; }
    }
}
