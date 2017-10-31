using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{
    public class TblStundensaetze : ITable<Stundensatz>
    {
        public const string TableName = "tblStundensatz";

        public string Uri { get; set; } = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<Stundensatz> Rows { get; set; }
    }
}
