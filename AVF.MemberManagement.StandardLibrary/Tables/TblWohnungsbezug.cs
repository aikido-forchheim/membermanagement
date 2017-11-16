using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{
    public class TblWohnungsbezug : ITable<Wohnungsbezug>
    {
        public const string TableName = "tblWohnungsbezug";

        public string Uri { get; set; } = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<Wohnungsbezug> Rows { get; set; }
    }
}
