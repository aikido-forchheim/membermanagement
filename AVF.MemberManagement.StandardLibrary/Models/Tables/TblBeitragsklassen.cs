using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Models.Tables
{
    public class TblBeitragsklassen : ITable<Beitragsklasse>
    {
        public const string TableName = "tblBeitragsklassen";

        [JsonProperty(PropertyName = TableName)]
        public List<Beitragsklasse> Rows { get; set; }
    }
}
