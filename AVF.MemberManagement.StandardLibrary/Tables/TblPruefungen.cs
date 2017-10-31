using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{
    public class TblPruefungen : ITable<Pruefung>
    {
        public const string TableName = "tbPruefung";

        public string Uri { get; set; } = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<Pruefung> Rows { get; set; }
    }
}
